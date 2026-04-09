namespace MonitoraAgro.API.Domain.Entities;

public class Device : EntityBase
{
    public int FieldId { get; set; }
    public Field Field { get; set; } = null!;
    public string TokenHash { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public uint ReadingIntervalInSeconds { get; set; }
    public IList<Sensor> Sensors { get; set; } = []; 
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}
