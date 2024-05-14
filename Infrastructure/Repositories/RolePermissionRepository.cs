using CSharpAuth.Domain.Entities;
using CSharpAuth.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CSharpAuth.Infrastructure.Repositories;

public class RolePermissionRepository : IRolePermissionRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IRepositoryBase<RolePermission> _repositoryBase;

    public RolePermissionRepository(ApplicationDbContext context, IRepositoryBase<RolePermission> repositoryBase)
    {
        _context = context;
        _repositoryBase = repositoryBase;
    }

    public async Task<IEnumerable<RolePermission>> GetAll()
    {
        return await _repositoryBase.GetAllActiveAsync();
    }

    public async Task<IEnumerable<RolePermission>> GetAllWithRoleAndPermission()
    {
        return await _context.RolePermissions
            .AsNoTracking()
                .Include(rp => rp.Role)
                .Include(rp => rp.Permission)
            .Where(rp => rp.DeletedAt == null && rp.Permission != null && rp.Role != null
                && rp.Role.DeletedAt == null
                && rp.Permission.DeletedAt == null)
            .ToListAsync();
    }

    public async Task<RolePermission?> GetByIdOrNull(Guid rolePermissionUuid)
    {
        return await _repositoryBase.GetActiveByIdAsync(rolePermissionUuid);
    }

    public async Task<RolePermission?> GetByIdWithRoleAndPermisssionOrNull(Guid rolePermissionUuid)
    {
        return await _context.RolePermissions
            .AsNoTracking()
                .Include(rp => rp.Role)
                .Include(rp => rp.Permission)
            .Where(rp => rp.Uuid == rolePermissionUuid && rp.DeletedAt == null && rp.Role != null && rp.Permission != null
                && rp.Role.DeletedAt == null
                && rp.Permission.DeletedAt == null)
            .SingleOrDefaultAsync();
    }

    public async Task<RolePermission?> GetByRoleAndPermissionOrNull(Guid roleUuid, Guid permissionUuid)
    {
        return await _context.RolePermissions
            .AsNoTracking()
                .Include(rp => rp.Role)
                .Include(rp => rp.Permission)
            .Where(rp => rp.DeletedAt == null
            && rp.RoleUuid == roleUuid && rp.PermissionUuid == permissionUuid && rp.Role != null && rp.Permission != null
                && rp.Role.DeletedAt == null
                && rp.Permission.DeletedAt == null)
            .SingleOrDefaultAsync();
    }

    public async Task<RolePermission> Create(RolePermission rolePermission)
    {
        return await _repositoryBase.CreateAsync(rolePermission);
    }

    public async Task SoftDelete(Guid rolePermissionUuid)
    {
        await _repositoryBase.SoftDeleteAsync(rolePermissionUuid);
    }
}
