using MonitoraAgro.API.Domain.Settings;

namespace MonitoraAgro.API.Application.DTOs.Requests;

public record PagedRequest(
    int PageNumber = PaginationRules.DEFAULT_PAGE_NUMBER, 
    int PageSize = PaginationRules.DEFAULT_PAGE_SIZE);