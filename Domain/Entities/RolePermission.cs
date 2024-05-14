namespace CSharpAuth.Domain.Entities;

public class RolePermission : EntityBase
{
    public required Guid RoleUuid { get; set; }
    public required Guid PermissionUuid { get; set; }
    public Role? Role { get; set; }
    public Permission? Permission { get; set; }
}
