using System.Net;

namespace MonitoraAgro.API.Application.Errors.Exceptions;

public class NotFoundException(string message) : MonitoraAgroException(message)
{
    public override IList<string> GetErrorMessages() => [Message];

    public override HttpStatusCode GetStatusCode() => HttpStatusCode.NotFound;
}
