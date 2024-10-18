using IPTvOnlinePlayer.Domain.Playlists;
using IPTvOnlinePlayer.Models.MediaItem;
using IPTvOnlinePlayer.Models.Playlist;
using IPTvOnlinePlayer.Services.Playlists;
using Microsoft.DotNet.Scaffolding.Shared.ProjectModel;

namespace IPTvOnlinePlayer.Factories;

public interface IPlaylistModelFactory
{
    Task<PlaylistModel> PreparePlaylistModel(PlaylistModel model, Playlist playlist);
    Task<PlaylistListModel> PreparePlaylistListModelAsync(PlaylistListModel model);
}
public class PlaylistModelFactory : IPlaylistModelFactory
{
    private readonly IMediaItemService _mediaItemService;
    private readonly IMediaCategoryService _mediaCategoryService;
    private readonly IPlaylistService _playlistService;
    private readonly IMediaItemModelFactory _mediaItemModelFactory;

    public PlaylistModelFactory(
        IMediaItemService mediaItemService,
        IMediaCategoryService mediaCategoryService,
        IPlaylistService playlistService, IMediaItemModelFactory mediaItemModelFactory)
    {
        _mediaItemService = mediaItemService;
        _mediaCategoryService = mediaCategoryService;
        _playlistService = playlistService;
        _mediaItemModelFactory = mediaItemModelFactory;
    }

    public async Task<PlaylistModel> PreparePlaylistModel(PlaylistModel model, Playlist playlist)
    {
        if (model == null)
            throw new ArgumentNullException(nameof(model));
        
        if (playlist == null)
            throw new ArgumentNullException(nameof(playlist));
        
        //ToDo: use automapper instead of manual mapping
        model.Id = playlist.Id;
        model.Name = playlist.Name;
        model.CreatedOnUtc = playlist.CreatedOnUtc;
        model.LogoUrl = playlist.LogoUrl;
        
        model.CategoryCount = playlist.CategoryCount;
        model.MediaItemCount = playlist.MediaItemCount;
        
        return model;
    }

    public async Task<PlaylistListModel> PreparePlaylistListModelAsync(PlaylistListModel model)
    {
        if (model == null)
            throw new ArgumentNullException(nameof(model));
        
        var playlists = await _playlistService.GetAllPlaylistsAsync();
        
        foreach (var playlist in playlists)
        {
            var playlistModel = new PlaylistModel();
            await PreparePlaylistModel(playlistModel, playlist);
            model.Playlists.Add(playlistModel);
        }
        
        model.Playlists = model.Playlists.OrderByDescending(p => p.DisplayOrder).ThenBy(x => x.CreatedOnUtc).ToList();

        var favoriteMediaItems = await _mediaItemService.GetAllMediaItemsAsync(showOnlyFavorite: true);

        foreach (var mediaItem in favoriteMediaItems)
        {
            model.FavoriteMediaItems.Add(await _mediaItemModelFactory.PrepareMediaItemModelAsync(new MediaItemModel(), mediaItem, true));
        }
        
        return model;
    }
}