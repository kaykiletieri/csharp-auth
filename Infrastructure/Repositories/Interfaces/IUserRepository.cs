using CSharpAuth.Domain.Entities;

namespace CSharpAuth.Infrastructure.Repositories.Interfaces;

public interface IUserRepository
{
    Task<IEnumerable<User>> GetAll();
    Task<IEnumerable<User>> GetAllWithRoles();
    Task<User?> GetByIdOrNull(Guid userUuid);
    Task<User?> GetByIdWithRolesOrNull(Guid userUuid);
    Task<User?> GetByUsernameOrNull(string username);
    Task<User> CreateAsync(User user);
    Task<User> UpdateAsync(User user);
    Task SoftDeleteAsync(Guid userUuid);
}