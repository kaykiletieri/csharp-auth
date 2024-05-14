using CSharpAuth.Domain.Entities;
using CSharpAuth.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CSharpAuth.Infrastructure.Repositories;

public class RepositoryBase<TEntity> : IRepositoryBase<TEntity> where TEntity : EntityBase
{
    protected readonly ApplicationDbContext _context;
    protected readonly DbSet<TEntity> _dbSet;

    public RepositoryBase(ApplicationDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<TEntity>();
    }

    public async Task<IEnumerable<TEntity>> GetAllActiveAsync()
    {
        return await _dbSet.AsNoTracking().Where(e => e.DeletedAt == null).ToListAsync();
    }

    public async Task<TEntity?> GetActiveByIdAsync(Guid uuid)
    {
        return await _dbSet.AsNoTracking().FirstOrDefaultAsync(e => e.Uuid == uuid && e.DeletedAt == null);
    }

    public async Task<TEntity> CreateAsync(TEntity entity)
    {
        if (entity == null)
        {
            throw new ArgumentException("Entity cannot be null");
        }

        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();

        return entity;
    }

    public async Task<TEntity> UpdateAsync(TEntity entity)
    {
        if (entity == null)
        {
            throw new ArgumentException("Entity cannot be null");
        }

        TEntity? existingEntity = await GetActiveByIdAsync(entity.Uuid) ?? throw new InvalidOperationException($"Entity with UUID {entity.Uuid} not found");

        entity.CreatedAt = existingEntity.CreatedAt;
        entity.UpdatedAt = DateTime.UtcNow;

        _dbSet.Update(entity);
        await _context.SaveChangesAsync();

        return entity;
    }

    public async Task SoftDeleteAsync(Guid uuid)
    {
        TEntity entity = await GetActiveByIdAsync(uuid) ?? throw new InvalidOperationException($"Entity with UUID {uuid} not found");

        entity.DeletedAt = DateTime.UtcNow;

        _dbSet.Update(entity);
        await _context.SaveChangesAsync();
    }
}
