using CSharpAuth.Domain.Entities;
using CSharpAuth.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CSharpAuth.Infrastructure.Repositories;

public class PermissionRepository : IPermissionRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IRepositoryBase<Permission> _repositoryBase;

    public PermissionRepository(ApplicationDbContext context, IRepositoryBase<Permission> repositoryBase)
    {
        _context = context;
        _repositoryBase = repositoryBase;
    }

    public async Task<IEnumerable<Permission>> GetAll()
    {
        return await _repositoryBase.GetAllActiveAsync();
    }

    public async Task<IEnumerable<Permission>> GetAllWithRoles()
    {
        return await _context.Permissions
            .AsNoTracking()
                .Include(p => p.RolePermissions)
                    .ThenInclude(rp => rp.Role)
            .Where(p => p.DeletedAt == null
                && p.RolePermissions.All(rp => rp.DeletedAt == null && rp.Permission != null
                    && rp.Permission.DeletedAt == null))
            .ToListAsync();
    }

    public async Task<Permission?> GetByIdOrNull(Guid permissionUuid)
    {
        return await _repositoryBase.GetActiveByIdAsync(permissionUuid);
    }

    public async Task<Permission?> GetByIdWithRolesOrNull(Guid permissionUuid)
    {
        return await _context.Permissions
            .AsNoTracking()
                .Include(p => p.RolePermissions)
                    .ThenInclude(rp => rp.Role)
            .Where(p => p.Uuid == permissionUuid && p.DeletedAt == null
                && p.RolePermissions.All(rp => rp.DeletedAt == null && rp.Permission != null
                    && rp.Permission.DeletedAt == null))
            .SingleOrDefaultAsync();
    }

    public async Task<Permission?> GetByNameOrNull(string permissionName)
    {
        return await _context.Permissions
            .AsNoTracking()
            .SingleOrDefaultAsync(p => p.Name == permissionName && p.DeletedAt == null);
    }

    public async Task<Permission?> GetByNameWithRolesOrNull(string permissionName)
    {
        return await _context.Permissions
            .AsNoTracking()
                .Include(p => p.RolePermissions)
                    .ThenInclude(rp => rp.Role)
            .Where(p => p.Name == permissionName && p.DeletedAt == null
                && p.RolePermissions.All(rp => rp.DeletedAt == null && rp.Permission != null
                    && rp.Permission.DeletedAt == null))
            .SingleOrDefaultAsync();
    }

    public async Task<Permission> Create(Permission permission)
    {
        return await _repositoryBase.CreateAsync(permission);
    }

    public async Task<Permission> Update(Permission permission)
    {
        return await _repositoryBase.UpdateAsync(permission);
    }

    public async Task SoftDelete(Guid permissionUuid)
    {
        await _repositoryBase.SoftDeleteAsync(permissionUuid);
    }
}
