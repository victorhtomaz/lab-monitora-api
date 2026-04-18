namespace MonitoraAgro.API.Domain.Entities;

public class Sensor : EntityBase
{
    public int DeviceId { get; set; }
    public string InternalKey { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string Unit { get; set; } = string.Empty;
    public Threshold Threshold { get; set; } = default!;
}
