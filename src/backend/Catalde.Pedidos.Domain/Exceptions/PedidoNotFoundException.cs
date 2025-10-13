using System.Net;

namespace Catalde.Pedidos.Domain.Exceptions;

public class PedidoNotFoundException : DomainExceptionBase
{
    public PedidoNotFoundException(int pedidoId) : base($"Id {pedidoId} não encontrado", HttpStatusCode.NotFound){}
}
