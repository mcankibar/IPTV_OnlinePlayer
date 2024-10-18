using IPTvOnlinePlayer.Domain.Playlists;
using IPTvOnlinePlayer.Models.MediaCategory;
using IPTvOnlinePlayer.Services.Playlists;

namespace IPTvOnlinePlayer.Factories;

public class MediaCategoryModelFactory : IMediaCategoryModelFactory
{
    private readonly IMediaCategoryService _mediaCategoryService;
    private readonly IMediaItemService _mediaItemService;

    public MediaCategoryModelFactory(
        IMediaCategoryService mediaCategoryService,
        IMediaItemService mediaItemService)
    {
        _mediaCategoryService = mediaCategoryService;
        _mediaItemService = mediaItemService;
    }

    public async Task<MediaCategoryModel> PrepareMediaCategoryModel(MediaCategoryModel model, MediaCategory mediaCategory, bool prepareMediaItems = false)
    {
        if (model == null)
            throw new ArgumentNullException(nameof(model));
        
        if (mediaCategory == null)
            throw new ArgumentNullException(nameof(mediaCategory));

        model.Id = mediaCategory.Id;
        model.Name = mediaCategory.Name;
        model.DisplayOrder = mediaCategory.DisplayOrder;
        model.Published = mediaCategory.Published;
        model.LogoUrl  = mediaCategory.LogoUrl;
        
        var mediaItems = await _mediaItemService.GetAllMediaItemsAsync(categoryId: mediaCategory.Id);
        
        model.MediaCount = mediaItems.Count;

        return model;
    }

    public async Task<MediaCategoryListModel> PrepareMediaCategoryListModelAsync(MediaCategoryListModel model, Playlist playlist)
    {
        if (model == null)
            throw new ArgumentNullException(nameof(model));
        
        if (playlist == null)
            throw new ArgumentNullException(nameof(playlist));
        
        model.PlaylistName = playlist.Name;
        
        var categories = await _mediaCategoryService.GetAllMediaCategoriesAsync(playlistId: playlist.Id);

        foreach (var category in categories)
        {
            var tempCategoryModel = await PrepareMediaCategoryModel(new MediaCategoryModel(), category);
            
            model.Categories.Add(tempCategoryModel);
        }
        
        return model;
    }
}

public interface IMediaCategoryModelFactory
{
    Task<MediaCategoryModel> PrepareMediaCategoryModel(MediaCategoryModel model, MediaCategory mediaCategory, bool prepareMediaItems = false);
    Task<MediaCategoryListModel> PrepareMediaCategoryListModelAsync(MediaCategoryListModel model, Playlist playlist);
}