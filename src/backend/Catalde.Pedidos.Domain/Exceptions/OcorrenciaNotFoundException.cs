using System.Net;

namespace Catalde.Pedidos.Domain.Exceptions;

public class OcorrenciaNotFoundException : DomainExceptionBase
{
    
    public OcorrenciaNotFoundException(int pedidoId, int ocorrenciaId) : base($"Ocorrência {ocorrenciaId} não encontrada no pedido {pedidoId}", HttpStatusCode.NotFound)
    {
    }
}
