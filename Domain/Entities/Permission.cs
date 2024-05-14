namespace CSharpAuth.Domain.Entities;

public class Permission : EntityBase
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public ICollection<RolePermission> RolePermissions { get; set; } = [];
}