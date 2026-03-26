using System.Net;

namespace MonitoraAgro.API.Application.Errors.Exceptions;

public class ErrorOnValidationException(IList<string> errorMessages) : MonitoraAgroException(string.Empty)
{
    private readonly IList<string> _errorMessages = errorMessages;

    public override IList<string> GetErrorMessages() => _errorMessages;

    public override HttpStatusCode GetStatusCode() => HttpStatusCode.BadRequest;
}
