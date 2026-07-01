namespace MonitoraAgro.API.Domain.DTOs;

public record TimeSeriesDataResult(
    double Average,
    IEnumerable<TimeSeriesPoint> Points);
