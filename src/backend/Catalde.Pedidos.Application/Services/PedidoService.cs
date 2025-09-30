using Catalde.Pedidos.Application.DTOs;
using Catalde.Pedidos.Domain.Entities;
using Catalde.Pedidos.Domain.Enums;
using Catalde.Pedidos.Domain.ValueObjects;
using System.Runtime.CompilerServices;

namespace Catalde.Pedidos.Application.Services;

public class PedidoService : IPedidoService
{
    public Pedido CriarPedido(CriarPedidoDTO pedido)
    {
        EnsureNotNull(pedido);

        var numeroPedido = new NumeroPedido(pedido.NumeroPedido);

        return new Pedido(numeroPedido);
    }
    public void AdicionarOcorrencia(Pedido pedido, AdicionarOcorrenciaDTO ocorrenciaDto)
    {
        EnsureNotNull(pedido);
        EnsureNotNull(ocorrenciaDto);

        var ocorrencia = new Ocorrencia((ETipoOcorrencia)ocorrenciaDto.TipoOcorrencia, false);
        pedido.AdicionarOcorrencia(ocorrencia);
    }
    public PedidoDTO MapearParaDTO(Pedido pedido)
    {
        EnsureNotNull(pedido);

        return PedidoDTO.FromEntity(pedido);
    }
    private void EnsureNotNull<T>(T obj, [CallerArgumentExpression("obj")] string parametro = null)
    {
        if (obj is null)
            throw new ArgumentNullException(parametro);
    }
}
