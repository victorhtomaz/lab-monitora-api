using MonitoraAgro.API.Domain.Enums;

namespace MonitoraAgro.API.Domain.Entities;

public class Field : EntityBase
{
    public int UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public FieldType FieldType { get; set; }
    public IList<Device> Devices { get; set; } = [];
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}
