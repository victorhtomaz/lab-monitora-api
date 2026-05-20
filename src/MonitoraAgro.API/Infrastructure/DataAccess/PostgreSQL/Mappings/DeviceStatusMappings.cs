using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MonitoraAgro.API.Domain.Entities;

namespace MonitoraAgro.API.Infrastructure.DataAccess.PostgreSQL.Mappings;

public class DeviceStatusMappings : IEntityTypeConfiguration<DeviceStatus>
{
    public void Configure(EntityTypeBuilder<DeviceStatus> builder)
    {
        builder.ToTable("device_status");

        builder.HasKey(d => d.Id);

        builder.Property(d => d.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(d => d.DeviceId)
            .HasColumnName("device_id")
            .HasColumnType("INTEGER")
            .IsRequired();

        builder.Property(d => d.LastRssi)
            .HasColumnName("last_rssi")
            .HasColumnType("SMALLINT")
            .IsRequired(false);

        builder.Property(d => d.LastSoc)
            .HasColumnName("last_soc")
            .HasColumnType("NUMERIC(5,2)")
            .IsRequired(false);

        builder.Property(d => d.LastSeenAt)
            .HasColumnName("last_seen_at")
            .HasColumnType("TIMESTAMPTZ")
            .IsRequired(false);

        builder.HasIndex(d => d.DeviceId)
            .IsUnique();
    }
}
