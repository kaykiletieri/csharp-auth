using CSharpAuth.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CSharpAuth.Infrastructure.Mappings;

public class UserMapping : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");

        builder.HasKey(u => u.Uuid);

        builder.Property(u => u.Uuid)
            .HasColumnName("uuid")
            .IsRequired();

        builder.Property(u => u.Username)
            .HasColumnName("username")
            .IsRequired();

        builder.Property(u => u.HashedPassword)
            .HasColumnName("hashed_password")
            .IsRequired();

        builder.Property(u => u.Salt)
            .HasColumnName("salt")
            .IsRequired();

        builder.Property(u => u.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(u => u.UpdatedAt)
            .HasColumnName("updated_at")
            .IsRequired();

        builder.Property(u => u.DeletedAt)
            .HasColumnName("deleted_at");
    }
}