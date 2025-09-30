using Catalde.Pedidos.Domain.Entities;

namespace Catalde.Pedidos.Application.DTOs
{
    public class PedidoDTO
    {
        public int IdPedido { get; private set; }
        public int NumeroPedido { get; private set; }
        public DateTime HorarioPedido { get; private set; }
        public bool IndEntregue { get; private set; }
        public List<OcorrenciaDTO> Ocorrencias { get; private set; } = new();

        public static PedidoDTO FromEntity(Pedido pedido)
        {
            return new PedidoDTO
            {
                IdPedido = pedido.IdPedido,
                NumeroPedido = pedido.NumeroPedido.Value,
                HorarioPedido = pedido.HorarioPedido,
                IndEntregue = pedido.IndEntregue,
                Ocorrencias = pedido.Ocorrencias
                    .Select(o => new OcorrenciaDTO
                    {
                        IdOcorrencia = o.IdOcorrencia,
                        TipoOcorrencia = o.TipoOcorrencia,
                        HoraOcorrencia = o.HoraOcorrencia,
                        IndFinalizadora = o.IndFinalizadora
                    })
                    .ToList()
            };
        }
    }
}
