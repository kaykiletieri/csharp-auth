namespace CSharpAuth.Application.DTOs;

public record CreateUpdatePermissionDTO(string Name, string? Description = null);
public record PermissionDTO(Guid Uuid, string Name, string? Description = null);