using Catalde.Pedidos.Application.DTOs;
using Catalde.Pedidos.Domain.Entities;
using Catalde.Pedidos.Domain.Enums;
using Catalde.Pedidos.Domain.Exceptions;
using Catalde.Pedidos.Domain.ValueObjects;
using Catalde.Pedidos.Infrastructure.Repositories.Interfaces;
using System.Runtime.CompilerServices;

namespace Catalde.Pedidos.Application.Services;

public class PedidoService : IPedidoService
{
    private readonly IUnitOfWork _unitOfWork;
    public PedidoService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<Pedido> CriarPedidoAsync(CriarPedidoDTO dto)
    {
        EnsureNotNull(dto);

        var numeroPedido = new NumeroPedido(dto.NumeroPedido);
        var pedido = new Pedido(numeroPedido);

        _unitOfWork.Pedidos.Create(pedido);
        await _unitOfWork.CommitAsync();

        return pedido;
    }
    public async Task AdicionarOcorrenciaAsync(int pedidoId, AdicionarOcorrenciaDTO dto)
    {
        EnsureNotNull(dto);

        var pedido = await _unitOfWork.Pedidos.GetByIdAsync(pedidoId);
        if (pedido is null)
            throw new PedidoNotFoundException(pedidoId);

        var ocorrencia = new Ocorrencia((ETipoOcorrencia)dto.TipoOcorrencia, false);
        pedido.AdicionarOcorrencia(ocorrencia); 

        _unitOfWork.Pedidos.Update(pedido);
        await _unitOfWork.CommitAsync();
    }

    public async Task<bool> ExcluirOcorrenciaAsync(int pedidoId, int ocorrenciaId)
    {
        var pedido = await _unitOfWork.Pedidos.GetByIdAsync(pedidoId);
        if (pedido is null)
            throw new PedidoNotFoundException(pedidoId);

        var ocorrencia = pedido.Ocorrencias.FirstOrDefault(o => o.IdOcorrencia == ocorrenciaId);
        if (ocorrencia is null)
            throw new OcorrenciaNotFoundException(pedidoId, ocorrenciaId);

        pedido.ExcluirOcorrencia(ocorrencia);

        _unitOfWork.Pedidos.Update(pedido);
        await _unitOfWork.CommitAsync();

        return true;

    }
    public PedidoDTO MapearParaDTO(Pedido pedido)
    {
        EnsureNotNull(pedido);

        return PedidoDTO.FromEntity(pedido);
    }
    public async Task<List<PedidoDTO>> GetAllPedidosAsync()
    {
        var pedidos = await _unitOfWork.Pedidos.GetAllAsync();

        return pedidos.Select(PedidoDTO.FromEntity).ToList();
    }
    public async Task<PedidoDTO?> GetPedidoByIdAsync(int id)
    {
        var pedido = await _unitOfWork.Pedidos.GetByIdAsync(id);

        if (pedido is null)
            throw new PedidoNotFoundException(id);

        return PedidoDTO.FromEntity(pedido);
    }
    private void EnsureNotNull<T>(T obj, [CallerArgumentExpression("obj")] string parametro = null)
    {
        if (obj is null)
            throw new ArgumentNullException(parametro);
    }

}
