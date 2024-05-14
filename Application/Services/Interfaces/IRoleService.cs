using CSharpAuth.Application.DTOs;

namespace CSharpAuth.Application.Services.Interfaces;

public interface IRoleService
{
    Task<IEnumerable<RoleDTO>> GetAllRoles();
    Task<IEnumerable<RoleWithPermissionsDTO>> GetAllRolesWithPermissions();
    Task<RoleDTO?> GetRoleByIdOrNull(Guid roleUuid);
    Task<RoleWithPermissionsDTO?> GetRoleByIdWithPermissionsOrNull(Guid roleUuid);
    Task<RoleDTO> CreateRole(CreateUpdateRoleDTO roleDTO);
    Task<RoleDTO> UpdateRole(Guid uuid, CreateUpdateRoleDTO roleDTO);
    Task AssingPermissionToRole(Guid roleUuid, Guid permissionUuid);
    Task RemovePermissionFromRole(Guid roleUuid, Guid permissionUuid);
    Task SoftDeleteRole(Guid roleUuid);
}
