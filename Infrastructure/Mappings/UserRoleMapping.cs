using CSharpAuth.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CSharpAuth.Infrastructure.Mappings;

public class UserRoleMapping : IEntityTypeConfiguration<UserRole>
{
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        builder.ToTable("user_role");

        builder.HasKey(ur => ur.Uuid);

        builder.Property(ur => ur.Uuid)
            .HasColumnName("uuid")
            .IsRequired();

        builder.Property(ur => ur.UserUuid)
            .HasColumnName("user_uuid")
            .IsRequired();

        builder.Property(ur => ur.RoleUuid)
            .HasColumnName("role_uuid")
            .IsRequired();

        builder.Property(ur => ur.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(ur => ur.UpdatedAt)
            .HasColumnName("updated_at")
            .IsRequired();

        builder.Property(ur => ur.DeletedAt)
            .HasColumnName("deleted_at");
    }
}