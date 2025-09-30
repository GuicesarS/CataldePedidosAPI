using Catalde.Pedidos.Application.DTOs;
using Catalde.Pedidos.Domain.Entities;

namespace Catalde.Pedidos.Application.Services;

public interface IPedidoService
{
    Pedido CriarPedido(CriarPedidoDTO criarPedido);
    void AdicionarOcorrencia(Pedido pedido, AdicionarOcorrenciaDTO ocorrencia);
    PedidoDTO MapearParaDTO(Pedido pedido);
}
