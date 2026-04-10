using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MonitoraAgro.API.Domain.Entities;

namespace MonitoraAgro.API.Infrastructure.DataAccess.PostgreSQL.Mappings;

public class NotificationTokenMapping : IEntityTypeConfiguration<NotificationToken>
{
    public void Configure(EntityTypeBuilder<NotificationToken> builder)
    {
        builder.ToTable("notification_tokens");

        builder.HasKey(n => n.Id);

        builder.Property(n => n.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(n => n.UserId)
            .HasColumnName("user_id")
            .HasColumnType("INTEGER")
            .IsRequired();

        builder.Property(n => n.InstallationId)
            .HasColumnName("installation_id")
            .HasColumnType("UUID")
            .IsRequired();

        builder.Property(n => n.Token)
            .HasColumnName("token")
            .HasColumnType("TEXT")
            .IsRequired();

        builder.Property(n => n.CreatedAt)
            .HasColumnName("created_at")
            .HasColumnType("TIMESTAMPTZ")
            .IsRequired();

        builder.Property(n => n.UpdatedAt)
            .HasColumnName("updated_at")
            .HasColumnType("TIMESTAMPTZ")
            .IsRequired();

        builder.HasIndex(n => n.InstallationId)
            .IsUnique();

        builder.HasIndex(n => n.Token)
            .IsUnique();

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(n => n.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
