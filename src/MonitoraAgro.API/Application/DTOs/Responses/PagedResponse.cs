namespace MonitoraAgro.API.Application.DTOs.Responses;

public record PagedResponse<T>(IReadOnlyCollection<T> Data, int PageNumber, int PageSize, int TotalCount)
{
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
}