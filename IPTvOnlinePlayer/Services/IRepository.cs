using IPTvOnlinePlayer.Domain.Playlists;

namespace IPTvOnlinePlayer.Services;

public interface IRepository<TEntity> where TEntity : class
{
    Task<TEntity> GetByIdAsync(int id);
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task AddAsync(TEntity entity);
    Task UpdateAsync(TEntity entity);
    Task DeleteAsync(int id);
    Task BulkInsertAsync(IList<TEntity> entities);
    Task BulkDeleteAsync(IList<TEntity> entities);
}