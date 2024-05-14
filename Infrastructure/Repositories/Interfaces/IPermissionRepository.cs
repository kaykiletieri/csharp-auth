using CSharpAuth.Domain.Entities;

namespace CSharpAuth.Infrastructure.Repositories.Interfaces;

public interface IPermissionRepository
{
    Task<IEnumerable<Permission>> GetAll();
    Task<IEnumerable<Permission>> GetAllWithRoles();
    Task<Permission?> GetByIdOrNull(Guid permissionUuid);
    Task<Permission?> GetByIdWithRolesOrNull(Guid permissionUuid);
    Task<Permission?> GetByNameOrNull(string permissionName);
    Task<Permission?> GetByNameWithRolesOrNull(string permissionName);
    Task<Permission> Create(Permission permission);
    Task<Permission> Update(Permission permission);
    Task SoftDelete(Guid permissionUuid);
}