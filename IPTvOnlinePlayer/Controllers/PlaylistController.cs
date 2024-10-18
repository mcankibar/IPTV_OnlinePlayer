using IPTvOnlinePlayer.Domain.Playlists;
using IPTvOnlinePlayer.Factories;
using IPTvOnlinePlayer.Models;
using IPTvOnlinePlayer.Models.M3U;
using IPTvOnlinePlayer.Models.MediaCategory;
using IPTvOnlinePlayer.Models.MediaItem;
using IPTvOnlinePlayer.Models.Playlist;
using IPTvOnlinePlayer.Services;
using IPTvOnlinePlayer.Services.Playlists;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IPTvOnlinePlayer.Controllers;

[Authorize]
public class PlaylistController : Controller
{
    private readonly IPlaylistModelFactory _playlistModelFactory;
    private readonly IPlaylistService _playlistService;
    private readonly IM3UService _m3UService;
    private readonly IMediaCategoryService _mediaCategoryService;
    private readonly IMediaItemService _mediaItemService;
    private readonly IMediaCategoryModelFactory _mediaCategoryModelFactory;
    private readonly IMediaItemModelFactory _mediaItemModelFactory;

    public PlaylistController(
        IPlaylistModelFactory playlistModelFactory,
        IM3UService m3UService,
        IPlaylistService playlistService,
        IMediaCategoryService mediaCategoryService,
        IMediaItemService mediaItemService,
        IMediaCategoryModelFactory mediaCategoryModelFactory,
        IMediaItemModelFactory mediaItemModelFactory)
    {
        _playlistModelFactory = playlistModelFactory;
        _m3UService = m3UService;
        _playlistService = playlistService;
        _mediaCategoryService = mediaCategoryService;
        _mediaItemService = mediaItemService;
        _mediaCategoryModelFactory = mediaCategoryModelFactory;
        _mediaItemModelFactory = mediaItemModelFactory;
    }

    public async Task<IActionResult> Index()
    {
        var model = await _playlistModelFactory.PreparePlaylistListModelAsync(new PlaylistListModel());

        return View(model);
    }

    public IActionResult Create()
    {
        var model = new PlaylistCreateModel();

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Create(PlaylistCreateModel model)
    {
        if ((model.UseUrl && string.IsNullOrEmpty(model.FileUrl)) || (!model.UseUrl && model.File == null))
        {
            ModelState.AddModelError(model.UseUrl ? "FileUrl" : "File",
                model.UseUrl ? "File Url is required" : "File is required");
        }

        if (!ModelState.IsValid)
        {
            return PartialView("Create", model);
        }

        var playlist = new Playlist
        {
            Name = model.Name,
            DisplayOrder = model.DisplayOrder,
            LogoUrl = string.IsNullOrEmpty(model.LogoUrl)
                ? "https://t4.ftcdn.net/jpg/02/36/41/51/360_F_236415197_aHuHuTomAaURIX0UKAGCdWkOGTO4qBfH.jpg"
                : model.LogoUrl,
        };

        await _playlistService.InsertPlaylistAsync(playlist);

        var mediaItems = GetMediaItems(model);
        
        var mediaCategories = await _mediaCategoryService.GetAllMediaCategoriesAsync(playlistId: playlist.Id);
        var mediaItemsToInsert = new List<MediaItem>();

        foreach (var mediaItem in mediaItems)
        {
            var mediaCategory = mediaCategories.FirstOrDefault(c => c.Name == mediaItem.GroupTitle);

            if (mediaCategory == null)
            {
                mediaCategory = await CreateMediaCategory(mediaItem.GroupTitle, playlist.Id);
                mediaCategories.Add(mediaCategory);
            }
                
            mediaItemsToInsert.Add(CreateMediaItem(mediaItem, mediaCategory.Id));
        }
        
        await _mediaItemService.InsertMediaItemsAsync(mediaItemsToInsert);

        playlist.MediaItemCount = mediaItems.Count;
        playlist.CategoryCount = mediaCategories.Count;
        await _playlistService.UpdatePlaylistAsync(playlist);

        return PartialView("CreateSuccess");
    }

    private async Task<MediaCategory> CreateMediaCategory(string categoryName, int playlistId)
    {
        var mediaCategory = new MediaCategory
        {
            Name = categoryName,
            DisplayOrder = 0,
            Published = true,
            PlaylistId = playlistId,
            LogoUrl = string.Empty
        };

        await _mediaCategoryService.InsertMediaCategoryAsync(mediaCategory);
        return mediaCategory;
    }

    private MediaItem CreateMediaItem(MediaItemImportModel mediaItem, int categoryId)
    {
        return new MediaItem
        {
            Name = mediaItem.Name,
            StreamUrl = mediaItem.Url,
            LogoUrl = mediaItem.TvgLogo,
            Published = true,
            DisplayOrder = 0,
            CategoryId = categoryId
        };
    }

    public async Task<IActionResult> Details(int id)
    {
        //TODO: add authorization

        var playlist = await _playlistService.GetPlaylistByIdAsync(id);

        if (playlist == null)
            return RedirectToAction("Index");

        var model = await _mediaCategoryModelFactory.PrepareMediaCategoryListModelAsync(new MediaCategoryListModel(),
            playlist);

        return View(model);
    }

    public async Task<IActionResult> ToggleMediaCategoryPublished(int id)
    {
        //TODO: add authorization
        var mediaCategory = await _mediaCategoryService.GetMediaCategoryByIdAsync(id);

        if (mediaCategory == null)
            throw new ArgumentNullException(nameof(mediaCategory));

        mediaCategory.Published = !mediaCategory.Published;

        await _mediaCategoryService.UpdateMediaCategoryAsync(mediaCategory);

        return Json(new { success = true });
    }
    [HttpPost]
    public async Task<IActionResult> ToggleMediaItemFavorite(int id)
    {
        //TODO: add authorization
        
        var mediaItem = await _mediaItemService.GetMediaItemByIdAsync(id);
        
        if (mediaItem == null)
            throw new ArgumentNullException(nameof(mediaItem));
        
        mediaItem.Favorite = !mediaItem.Favorite;
        
        await _mediaItemService.UpdateMediaItemAsync(mediaItem);
        
        return Json(new { success = true });
    }

    private List<MediaItemImportModel> GetMediaItems(PlaylistCreateModel model)
    {
        var content = model.UseUrl ? _m3UService.ProcessM3U(model.FileUrl) : _m3UService.ProcessM3U(model.File);

        var mediaItems = new List<MediaItemImportModel>();

        foreach (var media in content.Medias)
        {
            var tempMediaItem = new MediaItemImportModel
            {
                Name = media.Attributes.TvgName,
                Url = media.MediaFile,
                TvgName = media.Attributes.TvgName,
                TvgLogo = media.Attributes.TvgLogo,
                GroupTitle = media.Attributes.GroupTitle
            };

            mediaItems.Add(tempMediaItem);
        }

        return mediaItems;
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        //TODO: add authorization

        var playlist = await _playlistService.GetPlaylistByIdAsync(id);

        if (playlist == null)
            return RedirectToAction("Index");

        #region Delete Playlist

        var categoriesToDelete = await _mediaCategoryService.GetAllMediaCategoriesAsync(playlistId: playlist.Id);
        var mediaItemsToDelete = new List<MediaItem>();

        foreach (var category in categoriesToDelete)
        {
            var mediaItems = await _mediaItemService.GetAllMediaItemsAsync(categoryId: category.Id);

            mediaItemsToDelete.AddRange(mediaItems);
        }
        
        await _mediaItemService.DeleteMediaItemsAsync(mediaItemsToDelete);

        await _mediaCategoryService.DeleteMediaCategoriesAsync(categoriesToDelete);

        await _playlistService.DeletePlaylistAsync(id);

        #endregion

        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> RenderMediaItemList(int categoryId)
    {
        //TODO: add authorization

        var category = await _mediaCategoryService.GetMediaCategoryByIdAsync(categoryId);

        if (category == null)
            return RedirectToAction("Index");

        var model = await _mediaItemModelFactory.PrepareMediaItemListModelAsync(new MediaItemListModel(), category);

        return PartialView("/Views/Playlist/_CategoryMediaItemTable.cshtml", model);
    }
}