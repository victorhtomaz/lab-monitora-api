using MonitoraAgro.API.Domain.Enums;

namespace MonitoraAgro.API.Domain.ValueObjects;

public record MeasurementPeriod
{
    public DateTime StartDate { get; init; }
    public DateTime EndDate { get; init; }
    public string? AggregationWindow { get; init; }
    private MeasurementPeriod(DateTime startDate, DateTime endDate)
    {
        if (startDate > endDate)
            throw new ArgumentException("A data de início não pode ser maior que a data de término.");

        StartDate = startDate.ToUniversalTime();
        EndDate = endDate.ToUniversalTime();
        AggregationWindow = CalculateAggregationWindow(startDate, endDate);
    }
    public static MeasurementPeriod FromAvailablePeriod(AvailablePeriod period)
    {
        var end = DateTime.UtcNow;
        var start = period switch
        {
            AvailablePeriod.OneDay => end.AddDays(-1),
            AvailablePeriod.OneWeek => end.AddDays(-7),
            AvailablePeriod.OneMonth => end.AddMonths(-1),
            AvailablePeriod.OneSemester => end.AddMonths(-6),
            _ => end.AddDays(-1)
        };

        return new MeasurementPeriod(start, end);
    }

    public static MeasurementPeriod FromAbsoluteDates(DateTime start, DateTime? end)
    {
        return new MeasurementPeriod(start, end ?? DateTime.UtcNow);
    }

    private static string? CalculateAggregationWindow(DateTime start, DateTime end)
    {
        TimeSpan duration = end - start;
        return duration.TotalDays switch
        {
            <= 2 => null,
            <= 7 => "1h",
            <= 31 => "4h",
            _ => "1d"
        };
    }
}
