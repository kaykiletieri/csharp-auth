using CSharpAuth.Domain.Entities;
using CSharpAuth.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CSharpAuth.Infrastructure.Repositories;

public class RoleRepository : IRoleRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IRepositoryBase<Role> _repositoryBase;

    public RoleRepository(ApplicationDbContext context, IRepositoryBase<Role> repositoryBase)
    {
        _context = context;
        _repositoryBase = repositoryBase;
    }

    public async Task<IEnumerable<Role>> GetAll()
    {
        return await _repositoryBase.GetAllActiveAsync();
    }

    public async Task<IEnumerable<Role>> GetAllWithPermissions()
    {
        return await _context.Roles
            .AsNoTracking()
            .Include(r => r.RolePermissions)
                .ThenInclude(rp => rp.Permission)
            .Where(r => r.DeletedAt == null &&
                    (r.RolePermissions.Any(rp => rp.DeletedAt == null) ||
                    r.RolePermissions.All(rp => rp.DeletedAt == null &&
                        rp.Permission != null && rp.Permission.DeletedAt == null)))
            .ToListAsync();
    }

    public async Task<IEnumerable<Role>> GetAllWithUsers()
    {
        return await _context.Roles
            .AsNoTracking()
                .Include(r => r.UserRoles)
                    .ThenInclude(ur => ur.User)
            .Where(r => r.DeletedAt == null &&
                r.UserRoles.Any(ur => ur.DeletedAt == null) ||
                r.UserRoles.All(ur => ur.DeletedAt == null && ur.Role != null &&
                    ur.Role.DeletedAt == null))
            .ToListAsync();
    }

    public async Task<IEnumerable<Role>> GetAllWithPermissionsAndUsers()
    {
        return await _context.Roles
            .AsNoTracking()
                .Include(r => r.RolePermissions)
                    .ThenInclude(rp => rp.Permission)
                .Include(r => r.UserRoles)
                    .ThenInclude(ur => ur.User)
            .Where(r => r.DeletedAt == null &&
                r.RolePermissions.Any(rp => rp.DeletedAt == null) ||
                r.RolePermissions.All(rp => rp.DeletedAt == null && rp.Permission != null &&
                    rp.Permission.DeletedAt == null) &&
                r.UserRoles.Any(ur => ur.DeletedAt == null) ||
                r.UserRoles.All(ur => ur.DeletedAt == null && ur.Role != null &&
                    ur.Role.DeletedAt == null))
            .ToListAsync();
    }

    public async Task<Role?> GetByIdOrNull(Guid roleUuid)
    {
        return await _repositoryBase.GetActiveByIdAsync(roleUuid);
    }

    public async Task<Role?> GetByIdWithPermissionsOrNull(Guid roleUuid)
    {
        return await _context.Roles
            .AsNoTracking()
                .Include(r => r.RolePermissions)
                    .ThenInclude(rp => rp.Permission)
            .Where(r => r.Uuid == roleUuid && r.DeletedAt == null &&
                r.RolePermissions.Any(rp => rp.DeletedAt == null) ||
                r.RolePermissions.All(rp => rp.DeletedAt == null && rp.Permission != null &&
                    rp.Permission.DeletedAt == null))
            .SingleOrDefaultAsync();
    }

    public async Task<Role?> GetByNameOrNull(string roleName)
    {
        return await _context.Roles
            .AsNoTracking()
            .SingleOrDefaultAsync(r => r.Name == roleName && r.DeletedAt == null);
    }

    public async Task<Role?> GetByNameWithPermissionsOrNull(string roleName)
    {
        return await _context.Roles
            .AsNoTracking()
                .Include(r => r.RolePermissions)
                    .ThenInclude(rp => rp.Permission)
            .Where(r => r.Name == roleName && r.DeletedAt == null &&
                r.RolePermissions.Any(rp => rp.DeletedAt == null) ||
                r.RolePermissions.All(rp => rp.DeletedAt == null && rp.Permission != null &&
                    rp.Permission.DeletedAt == null))
            .SingleOrDefaultAsync();
    }

    public async Task<Role?> GetByNameWithUsersOrNull(string roleName)
    {
        return await _context.Roles
            .AsNoTracking()
                .Include(r => r.UserRoles)
                    .ThenInclude(ur => ur.User)
            .Where(r => r.Name == roleName && r.DeletedAt == null &&
                r.UserRoles.Any(ur => ur.DeletedAt == null) ||
                r.UserRoles.All(ur => ur.DeletedAt == null && ur.Role != null &&
                    ur.Role.DeletedAt == null))
            .SingleOrDefaultAsync();
    }

    public async Task<Role> Create(Role role)
    {
        return await _repositoryBase.CreateAsync(role);
    }

    public async Task<Role> Update(Role role)
    {
        return await _repositoryBase.UpdateAsync(role);
    }

    public async Task SoftDelete(Guid roleUuid)
    {
        await _repositoryBase.SoftDeleteAsync(roleUuid);
    }
}
