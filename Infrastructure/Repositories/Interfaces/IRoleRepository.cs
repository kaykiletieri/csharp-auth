using CSharpAuth.Domain.Entities;

namespace CSharpAuth.Infrastructure.Repositories.Interfaces;

public interface IRoleRepository
{
    Task<IEnumerable<Role>> GetAll();
    Task<IEnumerable<Role>> GetAllWithPermissions();
    Task<IEnumerable<Role>> GetAllWithUsers();
    Task<IEnumerable<Role>> GetAllWithPermissionsAndUsers();
    Task<Role?> GetByIdOrNull(Guid RoleUuid);
    Task<Role?> GetByIdWithPermissionsOrNull(Guid RoleUuid);
    Task<Role?> GetByNameOrNull(string roleName);
    Task<Role?> GetByNameWithPermissionsOrNull(string roleName);
    Task<Role?> GetByNameWithUsersOrNull(string roleName);
    Task<Role> Create(Role role);
    Task<Role> Update(Role role);
    Task SoftDelete(Guid RoleUuid);
}