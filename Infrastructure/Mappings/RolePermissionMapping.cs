using CSharpAuth.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CSharpAuth.Infrastructure.Mappings;

public class RolePermissionMapping : IEntityTypeConfiguration<RolePermission>
{
    public void Configure(EntityTypeBuilder<RolePermission> builder)
    {
        builder.ToTable("role_permission");

        builder.HasKey(rp => rp.Uuid);

        builder.Property(rp => rp.Uuid)
            .HasColumnName("uuid")
            .IsRequired();

        builder.Property(rp => rp.RoleUuid)
            .HasColumnName("role_uuid")
            .IsRequired();

        builder.Property(rp => rp.PermissionUuid)
            .HasColumnName("permission_uuid")
            .IsRequired();

        builder.Property(rp => rp.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(rp => rp.UpdatedAt)
            .HasColumnName("updated_at")
            .IsRequired();

        builder.Property(rp => rp.DeletedAt)
            .HasColumnName("deleted_at");
    }
}