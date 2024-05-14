namespace CSharpAuth.Domain.Entities;
public class User : EntityBase
{
    public required string Username { get; set; }
    public required string HashedPassword { get; set; }
    public required byte[] Salt { get; set; }
    public ICollection<UserRole> UserRoles { get; set; } = [];
}
