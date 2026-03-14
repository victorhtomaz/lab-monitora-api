namespace MonitoraAgro.API.Application.DTOs.Responses;

public record ErrorResponse(IList<string> Errors)
{
    public ErrorResponse(string error) : this([error]) { }
}
