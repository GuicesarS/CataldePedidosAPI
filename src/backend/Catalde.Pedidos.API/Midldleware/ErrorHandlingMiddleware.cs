using Catalde.Pedidos.Domain.Exceptions;
using System.Net;
using System.Text.Json;

namespace Catalde.Pedidos.Api.Midldleware;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;
    private readonly IHostEnvironment _environment;
    private readonly JsonSerializerOptions _jsonSerializerOptions;

    public ErrorHandlingMiddleware(
        RequestDelegate next, 
        ILogger<ErrorHandlingMiddleware> logger, 
        IHostEnvironment environment)
    {
        _next = next;
        _logger = logger;
        _environment = environment;
        _jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        _logger.LogError(exception, "Erro capturado pelo middleware: {Message}", exception.Message);

        context.Response.ContentType = "application/json";

        var response = CreateErrorResponse(exception);
        context.Response.StatusCode = GetStatusCode(exception);

        await context.Response.WriteAsync(JsonSerializer.Serialize(response, _jsonSerializerOptions));
    }

    private object CreateErrorResponse(Exception exception)
    {
        if(exception is DomainExceptionBase domainException)
        {
            if(domainException is ValidationException validationException)
            {
                return new
                {
                    Message = domainException.Message,
                    Errors = validationException.Errors,
                    TimesStamp = DateTime.UtcNow,
                };
            }

            return new
            {
                Message = domainException.Message,
                TimesStamp = DateTime.UtcNow,
            };
        }

        return new
        {
            Message = _environment.IsDevelopment() ? exception.Message : "Ocorreu um erro interno no servidor.",
            TimesStamp = DateTime.UtcNow
        };
    }
    private int GetStatusCode(Exception exception)
    {
        return exception switch
        { 
            DomainExceptionBase domainException => (int)domainException.StatusCode,
            _ => (int)HttpStatusCode.InternalServerError
        };
    }
}
