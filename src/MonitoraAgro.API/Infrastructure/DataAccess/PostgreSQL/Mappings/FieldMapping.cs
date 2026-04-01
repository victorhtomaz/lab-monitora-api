using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MonitoraAgro.API.Domain.Entities;

namespace MonitoraAgro.API.Infrastructure.DataAccess.PostgreSQL.Mappings;

public class FieldMapping : IEntityTypeConfiguration<Field>
{
    public void Configure(EntityTypeBuilder<Field> builder)
    {
        builder.ToTable("fields");

        builder.HasKey(f => f.Id);

        builder.Property(f => f.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(f => f.UserId)
            .HasColumnName("user_id")
            .HasColumnType("INTEGER")
            .IsRequired();

        builder.Property(f => f.Name)
            .HasColumnName("name")
            .HasColumnType("VARCHAR")
            .IsRequired();
        
        builder.Property(f => f.FieldType)
            .HasColumnName("field_type")
            .HasColumnType("VARCHAR")
            .HasConversion<string>()
            .IsRequired();

        builder.Property(f => f.IsActive)
            .HasColumnName("is_active")
            .HasColumnType("BOOLEAN")
            .IsRequired();

        builder.Property(f => f.CreatedAt)
            .HasColumnName("created_at")
            .HasColumnType("TIMESTAMPTZ")
            .IsRequired();

        builder.HasMany(f => f.Devices)
            .WithOne()
            .HasForeignKey(d => d.FieldId);
    }
}
