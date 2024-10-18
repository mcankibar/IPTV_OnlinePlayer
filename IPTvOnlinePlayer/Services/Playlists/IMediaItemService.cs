using IPTvOnlinePlayer.Domain.Playlists;

namespace IPTvOnlinePlayer.Services.Playlists;

public interface IMediaItemService
{
    Task<MediaItem> GetMediaItemByIdAsync(int id);
    Task<IList<MediaItem>> GetAllMediaItemsAsync(string name = null, int categoryId = 0, int playlistId = 0, bool showOnlyFavorite = false);
    Task InsertMediaItemAsync(MediaItem mediaItem);
    Task InsertMediaItemsAsync(IList<MediaItem> mediaItem);
    Task UpdateMediaItemAsync(MediaItem mediaItem);
    Task DeleteMediaItemAsync(int id);
    Task DeleteMediaItemsAsync(IList<MediaItem> mediaItems);
}

public class MediaItemService : IMediaItemService
{
    private readonly IRepository<MediaItem> _mediaItemRepository;

    public MediaItemService(IRepository<MediaItem> mediaItemRepository)
    {
        _mediaItemRepository = mediaItemRepository;
    }

    public async Task<MediaItem> GetMediaItemByIdAsync(int id)
    {
        return await _mediaItemRepository.GetByIdAsync(id);
    }

    public async Task<IList<MediaItem>> GetAllMediaItemsAsync(string name = null, int categoryId = 0, int playlistId = 0, bool showOnlyFavorite = false)
    {
        var mediaItems = await _mediaItemRepository.GetAllAsync();

        if (!string.IsNullOrEmpty(name))
        {
            mediaItems = mediaItems.Where(p => p.Name.Contains(name));
        }

        if (categoryId > 0)
        {
            mediaItems = mediaItems.Where(p => p.CategoryId == categoryId);
        }

        if (showOnlyFavorite)
        {
            mediaItems = mediaItems.Where(p => p.Favorite);
        }
        
        return mediaItems.ToList();
    }

    public async Task InsertMediaItemAsync(MediaItem mediaItem)
    {
        await _mediaItemRepository.AddAsync(mediaItem);
    }

    public async Task InsertMediaItemsAsync(IList<MediaItem> mediaItems)
    {
        await _mediaItemRepository.BulkInsertAsync(mediaItems);
    }

    public async Task UpdateMediaItemAsync(MediaItem mediaItem)
    {
        await _mediaItemRepository.UpdateAsync(mediaItem);
    }

    public async Task DeleteMediaItemAsync(int id)
    {
        await _mediaItemRepository.DeleteAsync(id);
    }
    
    public async Task DeleteMediaItemsAsync(IList<MediaItem> mediaItems)
    {
        await _mediaItemRepository.BulkDeleteAsync(mediaItems);
    }
}