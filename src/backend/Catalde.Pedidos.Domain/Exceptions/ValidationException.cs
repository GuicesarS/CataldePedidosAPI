using System.Net;

namespace Catalde.Pedidos.Domain.Exceptions;

public class ValidationException : DomainExceptionBase
{
    public List<string> Errors { get; set; }
    public ValidationException(List<string> errors) : base("Erro de Validação", HttpStatusCode.BadRequest)
    {
        Errors = errors;
    }
}
