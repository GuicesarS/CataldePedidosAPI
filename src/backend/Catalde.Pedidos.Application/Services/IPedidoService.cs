using Catalde.Pedidos.Application.DTOs;
using Catalde.Pedidos.Domain.Entities;

namespace Catalde.Pedidos.Application.Services;

public interface IPedidoService
{
    Task<Pedido> CriarPedidoAsync(CriarPedidoDTO criarPedido);
    Task AdicionarOcorrenciaAsync(int pedidoId, AdicionarOcorrenciaDTO ocorrencia);
    Task<bool> ExcluirOcorrenciaAsync(int pedidoId, int ocorrenciaId);
    PedidoDTO MapearParaDTO(Pedido pedido);
    Task<List<PedidoDTO>> GetAllPedidosAsync();
    Task<PedidoDTO?> GetPedidoByIdAsync(int id);
}
