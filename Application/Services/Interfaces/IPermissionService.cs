using CSharpAuth.Application.DTOs;

namespace CSharpAuth.Application.Services.Interfaces;

public interface IPermissionService
{
    Task<IEnumerable<PermissionDTO>> GetAllPermissions();
    Task<PermissionDTO?> GetPermissionByIdOrNull(Guid uuid);
    Task<PermissionDTO> CreatePermission(CreateUpdatePermissionDTO permissionDTO);
    Task<PermissionDTO> UpdatePermission(Guid uuid, CreateUpdatePermissionDTO permissionDTO);
    Task SoftDeletePermission(Guid uuid);
}
