using IPTvOnlinePlayer.Domain.Playlists;
using IPTvOnlinePlayer.Models.MediaItem;
using IPTvOnlinePlayer.Services.Playlists;

namespace IPTvOnlinePlayer.Factories;

public class MediaItemModelFactory : IMediaItemModelFactory
{
    #region Fields

    private readonly IMediaItemService _mediaItemService;
    private readonly IMediaCategoryService _mediaCategoryService;
    private readonly IPlaylistService _playlistService;

    #endregion

    #region Ctor

    public MediaItemModelFactory(IMediaItemService mediaItemService, IMediaCategoryService mediaCategoryService, IPlaylistService playlistService)
    {
        _mediaItemService = mediaItemService;
        _mediaCategoryService = mediaCategoryService;
        _playlistService = playlistService;
    }

    #endregion
    
    public async Task<MediaItemModel> PrepareMediaItemModelAsync(MediaItemModel model, MediaItem mediaItem, bool includeCategory = false)
    {
        if (model is null)
            throw new ArgumentNullException(nameof(model));
        
        if (mediaItem is null)
            throw new ArgumentNullException(nameof(mediaItem));
        
        model.Id = mediaItem.Id;
        model.Name = mediaItem.Name;
        model.StreamUrl = mediaItem.StreamUrl;
        model.LogoUrl = mediaItem.LogoUrl;
        model.Favorite = mediaItem.Favorite;
        
        if (includeCategory)
        {
            var category = await _mediaCategoryService.GetMediaCategoryByIdAsync(mediaItem.CategoryId);
            
            model.CategoryName = category is null ? string.Empty : category.Name;

            if (category is not null)
            {
                var playlist = await _playlistService.GetPlaylistByIdAsync(category.PlaylistId);
                model.PlaylistName = playlist is null ? string.Empty : playlist.Name;
            }
        }

        return model;
    }

    public async Task<MediaItemListModel> PrepareMediaItemListModelAsync(MediaItemListModel model, MediaCategory mediaCategory)
    {
        if (model is null)
            throw new ArgumentNullException(nameof(model));
        
        if (mediaCategory is null)
            throw new ArgumentNullException(nameof(mediaCategory));

        var mediaItems = await _mediaItemService.GetAllMediaItemsAsync(categoryId: mediaCategory.Id);

        foreach (var mediaItem in mediaItems)
        {
            model.MediaItems.Add(await PrepareMediaItemModelAsync(new MediaItemModel(), mediaItem));
        }
       
        return model;
    }
}

public interface IMediaItemModelFactory
{
    Task<MediaItemModel> PrepareMediaItemModelAsync(MediaItemModel model, MediaItem mediaItem,
        bool includeCategory = false);
    Task<MediaItemListModel> PrepareMediaItemListModelAsync(MediaItemListModel model, MediaCategory mediaCategory);
}