namespace CSharpAuth.Application.DTOs;

public record CreateUpdateRoleDTO(string Name, string? Description = null);
public record RoleDTO(Guid Uuid, string Name, string? Description = null);
public record RoleWithPermissionsDTO(Guid Uuid, string Name, IEnumerable<PermissionDTO> Permissions, string? Description = null);

