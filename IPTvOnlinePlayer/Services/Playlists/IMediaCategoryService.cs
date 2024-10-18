using IPTvOnlinePlayer.Domain.Playlists;

namespace IPTvOnlinePlayer.Services.Playlists;

public interface IMediaCategoryService
{
    Task<MediaCategory> GetMediaCategoryByIdAsync(int id);
    Task<IList<MediaCategory>> GetAllMediaCategoriesAsync(string name = null, int playlistId = 0);
    Task InsertMediaCategoryAsync(MediaCategory mediaCategory);
    Task InsertMediaCategoriesAsync(IList<MediaCategory> mediaCategories);
    Task UpdateMediaCategoryAsync(MediaCategory mediaCategory);
    Task DeleteMediaCategoryAsync(int id);
    Task<MediaCategory> GetMediaCategoryByNameAsync(string name, int playlistId);
    Task DeleteMediaCategoriesAsync(IList<MediaCategory> mediaCategories);
}

public class MediaCategoryService : IMediaCategoryService
{
    private readonly IRepository<MediaCategory> _mediaCategoryRepository;

    public MediaCategoryService(IRepository<MediaCategory> mediaCategoryRepository)
    {
        _mediaCategoryRepository = mediaCategoryRepository;
    }

    public async Task<MediaCategory> GetMediaCategoryByIdAsync(int id)
    {
        return await _mediaCategoryRepository.GetByIdAsync(id);
    }

    public async Task<IList<MediaCategory>> GetAllMediaCategoriesAsync(string name = null, int playlistId = 0)
    {
        var mediaCategories = await _mediaCategoryRepository.GetAllAsync();

        if (!string.IsNullOrEmpty(name))
        {
            mediaCategories = mediaCategories.Where(p => p.Name.Contains(name));
        }

        if (playlistId > 0)
        {
            mediaCategories = mediaCategories.Where(p => p.PlaylistId == playlistId);
        }

        return mediaCategories.ToList();
    }
    
    public async Task<MediaCategory> GetMediaCategoryByNameAsync(string name, int playlistId)
    {
        return (await GetAllMediaCategoriesAsync(name: name, playlistId: playlistId)).FirstOrDefault();
    }

    public async Task InsertMediaCategoryAsync(MediaCategory mediaCategory)
    {
        await _mediaCategoryRepository.AddAsync(mediaCategory);
    }

    public async Task InsertMediaCategoriesAsync(IList<MediaCategory> mediaCategories)
    {
        foreach (var mediaCategory in mediaCategories)
        {
            await InsertMediaCategoryAsync(mediaCategory);
        }
    }

    public async Task UpdateMediaCategoryAsync(MediaCategory mediaCategory)
    {
        await _mediaCategoryRepository.UpdateAsync(mediaCategory);
    }

    public async Task DeleteMediaCategoryAsync(int id)
    {
        await _mediaCategoryRepository.DeleteAsync(id);
    }
    
    public async Task DeleteMediaCategoriesAsync(IList<MediaCategory> mediaCategories)
    {
        await _mediaCategoryRepository.BulkDeleteAsync(mediaCategories);
    }
}
