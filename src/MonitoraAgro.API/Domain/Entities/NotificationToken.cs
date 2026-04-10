namespace MonitoraAgro.API.Domain.Entities;

public class NotificationToken : EntityBase
{
    public int UserId { get; set; }
    public Guid InstallationId { get; set; }
    public string Token { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

}
