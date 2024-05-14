namespace CSharpAuth.Domain.Entities;

public class UserRole : EntityBase
{
    public required Guid UserUuid { get; set; }
    public required Guid RoleUuid { get; set; }
    public User? User { get; set; }
    public Role? Role { get; set; }
}
