namespace CSharpAuth.Domain.Entities;

public class EntityBase
{
    public required Guid Uuid { get; set; }
    public required DateTime CreatedAt { get; set; }
    public required DateTime UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
}
