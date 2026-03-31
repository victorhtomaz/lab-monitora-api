using System.Net;

namespace MonitoraAgro.API.Application.Errors.Exceptions;

public class InvalidLoginException() : MonitoraAgroException(ErrorMessages.INVALID_LOGIN)
{
    public override IList<string> GetErrorMessages() => [Message];

    public override HttpStatusCode GetStatusCode() => HttpStatusCode.Unauthorized;
}
