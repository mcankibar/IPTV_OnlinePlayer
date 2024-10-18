using IPTvOnlinePlayer.Data;
using IPTvOnlinePlayer.Domain;
using IPTvOnlinePlayer.Domain.Playlists;
using Microsoft.EntityFrameworkCore;

namespace IPTvOnlinePlayer.Services;

public class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
{
    private readonly ApplicationDbContext _context;

    public Repository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<TEntity> GetByIdAsync(int id)
    {
        return await _context.Set<TEntity>().FindAsync(id);
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await _context.Set<TEntity>().ToListAsync();
    }

    public async Task AddAsync(TEntity entity)
    {
        entity.CreatedOnUtc = DateTime.UtcNow;
        entity.ModifiedOnUtc = DateTime.UtcNow;
        
        await  _context.Set<TEntity>().AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(TEntity entity)
    {
        entity.ModifiedOnUtc = DateTime.UtcNow;
        
        _context.Set<TEntity>().Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _context.Set<TEntity>().FindAsync(id);
        if (entity != null)
        {
            _context.Set<TEntity>().Remove(entity);
            await _context.SaveChangesAsync();
        }
    }

    public async Task BulkInsertAsync(IList<TEntity> entities)
    {
        foreach (var item in entities)
        {
            item.CreatedOnUtc = DateTime.UtcNow;
            item.ModifiedOnUtc = DateTime.UtcNow;
        }
        
        await _context.Set<TEntity>().AddRangeAsync(entities);

        await _context.SaveChangesAsync();
    }

    public async Task BulkDeleteAsync(IList<TEntity> entities)
    {
        _context.Set<TEntity>().RemoveRange(entities);

        await _context.SaveChangesAsync();
    }
}