using System.Net;

namespace MonitoraAgro.API.Application.Errors.Exceptions;

public abstract class MonitoraAgroException(string message) : Exception(message)
{
    public abstract IList<string> GetErrorMessages();
    public abstract HttpStatusCode GetStatusCode();
}
