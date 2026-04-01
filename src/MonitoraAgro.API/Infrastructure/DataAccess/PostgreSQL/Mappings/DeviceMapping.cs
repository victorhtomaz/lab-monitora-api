using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MonitoraAgro.API.Domain.Entities;

namespace MonitoraAgro.API.Infrastructure.DataAccess.PostgreSQL.Mappings;

public class DeviceMapping : IEntityTypeConfiguration<Device>
{
    public void Configure(EntityTypeBuilder<Device> builder)
    {
        builder.ToTable("devices");

        builder.HasKey(d => d.Id);

        builder.Property(d => d.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(d => d.FieldId)
            .HasColumnName("field_id")
            .HasColumnType("INTEGER")
            .IsRequired();

        builder.Property(d => d.TokenHash)
            .HasColumnName("token_hash")
            .HasColumnType("VARCHAR")
            .IsRequired();

        builder.Property(d => d.DisplayName)
            .HasColumnName("display_name")
            .HasColumnType("VARCHAR")
            .IsRequired();

        builder.Property(d => d.ReadingIntervalInSeconds)
            .HasColumnName("reading_interval_in_seconds")
            .HasColumnType("INTEGER")
            .IsRequired();

        builder.Property(d => d.IsActive)
            .HasColumnName("is_active")
            .HasColumnType("BOOLEAN")
            .IsRequired();

        builder.Property(d => d.CreatedAt)
            .HasColumnName("created_at")
            .HasColumnType("TIMESTAMPTZ")
            .IsRequired();

        builder.HasMany(d => d.Sensors)
            .WithOne()
            .HasForeignKey(s => s.DeviceId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
