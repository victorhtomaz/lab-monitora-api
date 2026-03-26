using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MonitoraAgro.API.Domain.Entities;

namespace MonitoraAgro.API.Infrastructure.DataAccess.PostgreSQL.Mappings;

public class UserMapping : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(u => u.FirstName)
            .HasColumnName("first_name")
            .HasColumnType("VARCHAR")
            .IsRequired();

        builder.Property(u => u.LastName)
            .HasColumnName("last_name")
            .HasColumnType("VARCHAR")
            .IsRequired();

        builder.Property(u => u.Email)
            .HasColumnName("email")
            .HasColumnType("VARCHAR")
            .IsRequired();

        builder.Property(u => u.PasswordHash)
            .HasColumnName("password_hash")
            .HasColumnType("VARCHAR")
            .IsRequired();

        builder.Property(u => u.IsActive)
            .HasColumnName("is_active")
            .HasColumnType("BOOLEAN");

        builder.Property(u => u.CreatedAt)
            .HasColumnName("created_at")
            .HasColumnType("TIMESTAMPTZ");

        builder.HasIndex(u => u.Email)
            .IsUnique();
    }
}
