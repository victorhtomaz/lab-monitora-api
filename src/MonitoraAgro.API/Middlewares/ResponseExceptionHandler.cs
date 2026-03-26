using Microsoft.AspNetCore.Diagnostics;
using MonitoraAgro.API.Application.DTOs.Responses;
using MonitoraAgro.API.Application.Errors;
using MonitoraAgro.API.Application.Errors.Exceptions;

namespace MonitoraAgro.API.Middlewares;

public class ResponseExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is MonitoraAgroException monitoraAgroException)
        {
            httpContext.Response.StatusCode = (int)monitoraAgroException.GetStatusCode();

            var response = new ErrorResponse(monitoraAgroException.GetErrorMessages());

            await httpContext.Response.WriteAsJsonAsync(response, cancellationToken: cancellationToken);
        }
        else
        {
            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

            var response = new ErrorResponse(ErrorMessages.UNKNOWN_ERROR);

            await httpContext.Response.WriteAsJsonAsync(response, cancellationToken: cancellationToken);
        }

        return true;
    }
}
