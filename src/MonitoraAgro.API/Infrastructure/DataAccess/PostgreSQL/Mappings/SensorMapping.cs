using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MonitoraAgro.API.Domain.Entities;

namespace MonitoraAgro.API.Infrastructure.DataAccess.PostgreSQL.Mappings;

public class SensorMapping : IEntityTypeConfiguration<Sensor>
{
    public void Configure(EntityTypeBuilder<Sensor> builder)
    {
        builder.ToTable("sensors");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(s => s.DeviceId)
            .HasColumnName("device_id")
            .HasColumnType("INTEGER")
            .IsRequired();

        builder.Property(s => s.InternalKey)
            .HasColumnName("internal_key")
            .HasColumnType("VARCHAR")
            .IsRequired();

        builder.Property(s => s.DisplayName)
            .HasColumnName("display_name")
            .HasColumnType("VARCHAR")
            .IsRequired();

        builder.HasIndex(s => new { s.DeviceId, s.InternalKey})
            .IsUnique();

        builder.Property(s => s.Unit)
            .HasColumnName("unit")
            .HasColumnType("VARCHAR")
            .IsRequired();

        builder.HasOne(s => s.Threshold)
            .WithOne()
            .HasForeignKey<Threshold>(t => t.SensorId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
