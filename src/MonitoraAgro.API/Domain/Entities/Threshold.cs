namespace MonitoraAgro.API.Domain.Entities;

public class Threshold : EntityBase
{
    public int SensorId { get; set; }
    public double? LowerLimit { get; set; }
    public double? UpperLimit { get; set; }
    public bool NotificationEnabled { get; set; }
    public DateTime? NotifiedAt { get; set; }
}
