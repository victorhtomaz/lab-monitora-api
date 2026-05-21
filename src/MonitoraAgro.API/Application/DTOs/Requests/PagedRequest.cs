using MonitoraAgro.API.Domain.Settings;

namespace MonitoraAgro.API.Application.DTOs.Requests;

public record PagedRequest(
    int PageNumber = PaginationRules.DEFAULT_PAGE_NUMBER,
    int PageSize = PaginationRules.DEFAULT_PAGE_SIZE)
{
    public int PageNumber { get; init; } = PageNumber <= 0 ? PaginationRules.DEFAULT_PAGE_NUMBER : PageNumber;
    public int PageSize { get; init; } = PageSize <= 0 ? PaginationRules.DEFAULT_PAGE_SIZE : PageSize;
}