using IPTvOnlinePlayer.Factories;
using IPTvOnlinePlayer.Models.Playlist;
using IPTvOnlinePlayer.Services.Playlists;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IPTvOnlinePlayer.Controllers;

[Authorize]
public class PlayerController  : Controller
{
    private readonly IPlaylistModelFactory _playlistModelFactory;
    private readonly IPlaylistService _playlistService;

    public PlayerController(
        IPlaylistModelFactory playlistModelFactory,
        IPlaylistService playlistService)
    {
        _playlistModelFactory = playlistModelFactory;
        _playlistService = playlistService;
    }

    public async Task<IActionResult> Index()
    {
        var model = await _playlistModelFactory.PreparePlaylistListModelAsync(new PlaylistListModel());

        return View(model);
    }
    
    public async Task<IActionResult> Categories(int playlistId)
    {
        //Todo: Add authorization check
        
        var playlist = await _playlistService.GetPlaylistByIdAsync(playlistId);
        
        if (playlist == null)
            return RedirectToAction("Index");
        
        
        
        return View();
    }
}