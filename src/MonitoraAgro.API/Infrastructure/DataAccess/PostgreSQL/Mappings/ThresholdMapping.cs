using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MonitoraAgro.API.Domain.Entities;

namespace MonitoraAgro.API.Infrastructure.DataAccess.PostgreSQL.Mappings;

public class ThresholdMapping : IEntityTypeConfiguration<Threshold>
{
    public void Configure(EntityTypeBuilder<Threshold> builder)
    {
        builder.ToTable("thresholds");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(t => t.SensorId)
            .HasColumnName("sensor_id")
            .HasColumnType("INTEGER")
            .IsRequired();

        builder.Property(t => t.LowerLimit)
            .HasColumnName("lower_limit")
            .HasPrecision(10, 4)
            .IsRequired(false);

        builder.Property(t => t.UpperLimit)
            .HasColumnName("upper_limit")
            .HasPrecision(10, 4)
            .IsRequired(false);

        builder.Property(t => t.NotificationEnabled)
            .HasColumnName("notification_enabled")
            .HasColumnType("BOOLEAN")
            .IsRequired();

        builder.Property(t => t.NotifiedAt)
            .HasColumnName("notified_at")
            .HasColumnType("TIMESTAMPTZ")
            .IsRequired(false);
    }
}
