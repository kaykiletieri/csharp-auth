using CSharpAuth.Domain.Entities;

namespace CSharpAuth.Infrastructure.Repositories.Interfaces;

public interface IRolePermissionRepository
{
    Task<IEnumerable<RolePermission>> GetAll();
    Task<IEnumerable<RolePermission>> GetAllWithRoleAndPermission();
    Task<RolePermission?> GetByIdOrNull(Guid rolePermissionUuid);
    Task<RolePermission?> GetByIdWithRoleAndPermisssionOrNull(Guid rolePermissionUuid);
    Task<RolePermission?> GetByRoleAndPermissionOrNull(Guid roleUuid, Guid permissionUuid);
    Task<RolePermission> Create(RolePermission rolePermission);
    Task SoftDelete(Guid rolePermissionUuid);
}
