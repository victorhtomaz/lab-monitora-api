using MonitoraAgro.API.Domain.Enums;

namespace MonitoraAgro.API.Application.DTOs.Requests;

public record MeasurementFilterRequest(
    AvailablePeriod? Period,
    DateTime? StartDate,
    DateTime? EndDate);