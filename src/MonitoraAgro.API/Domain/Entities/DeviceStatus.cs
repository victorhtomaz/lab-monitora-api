namespace MonitoraAgro.API.Domain.Entities;

public class DeviceStatus : EntityBase
{
    public int DeviceId { get; set; }
    public int? LastRssi { get; set; }
    public double? LastSoc { get; set; }
    public DateTime? LastSeenAt { get; set; }
}
