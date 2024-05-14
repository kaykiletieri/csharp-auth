using CSharpAuth.Domain.Entities;
using CSharpAuth.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CSharpAuth.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IRepositoryBase<User> _repositoryBase;

    public UserRepository(ApplicationDbContext context, IRepositoryBase<User> repositoryBase)
    {
        _context = context;
        _repositoryBase = repositoryBase;
    }

    public async Task<IEnumerable<User>> GetAll()
    {
        return await _repositoryBase.GetAllActiveAsync();
    }

    public async Task<IEnumerable<User>> GetAllWithRoles()
    {
        return await _context.Users
            .AsNoTracking()
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
            .Where(u => u.DeletedAt == null &&
            u.UserRoles.Any(ur => ur.DeletedAt == null) ||
            u.UserRoles.All(ur => ur.DeletedAt == null && ur.Role != null
                && ur.Role.DeletedAt == null))
            .ToListAsync();
    }

    public async Task<User?> GetByIdOrNull(Guid userUuid)
    {
        return await _repositoryBase.GetActiveByIdAsync(userUuid);
    }

    public async Task<User?> GetByIdWithRolesOrNull(Guid userUuid)
    {
        return await _context.Users
            .AsNoTracking()
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
            .Where(u => u.Uuid == userUuid && u.DeletedAt == null &&
            u.UserRoles.Any(ur => ur.DeletedAt == null) ||
            u.UserRoles.All(ur => ur.DeletedAt == null && ur.Role != null
                && ur.Role.DeletedAt == null))
            .SingleOrDefaultAsync();
    }

    public async Task<User?> GetByUsernameOrNull(string username)
    {
        return await _context.Users
            .AsNoTracking()
            .Where(u => u.DeletedAt == null)
            .FirstOrDefaultAsync(u => u.Username == username);
    }

    public async Task<User> CreateAsync(User user)
    {
        return await _repositoryBase.CreateAsync(user);
    }

    public async Task<User> UpdateAsync(User user)
    {
        return await _repositoryBase.UpdateAsync(user);
    }

    public async Task SoftDeleteAsync(Guid userUuid)
    {
        await _repositoryBase.SoftDeleteAsync(userUuid);
    }
}
