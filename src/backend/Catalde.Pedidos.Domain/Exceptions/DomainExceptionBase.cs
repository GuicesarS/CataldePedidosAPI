using System.Net;

namespace Catalde.Pedidos.Domain.Exceptions;

public abstract class DomainExceptionBase : Exception
{
    public HttpStatusCode StatusCode { get; }
    protected DomainExceptionBase(
        string message,
        HttpStatusCode statusCode = HttpStatusCode.BadRequest) : base(message)
    {
        StatusCode = statusCode;
    }

    protected DomainExceptionBase(string message,
        Exception innerException,
        HttpStatusCode statusCode = HttpStatusCode.BadRequest) : base(message, innerException)
    {
        StatusCode = statusCode;
    }
}
