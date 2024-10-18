using IPTvOnlinePlayer.Domain.Playlists;
using System.Linq;

namespace IPTvOnlinePlayer.Services.Playlists;

public interface IPlaylistService
{
    Task<Playlist> GetPlaylistByIdAsync(int id);
    Task<IList<Playlist>> GetAllPlaylistsAsync(string name = null);
    Task InsertPlaylistAsync(Playlist playlist);
    Task InsertPlaylistsAsync(IList<Playlist> playlist);
    Task UpdatePlaylistAsync(Playlist playlist);
    Task DeletePlaylistAsync(int id);
}

public class PlaylistService : IPlaylistService
{
    private readonly IRepository<Playlist> _playlistRepository;

    public PlaylistService(IRepository<Playlist> playlistRepository)
    {
        _playlistRepository = playlistRepository;
    }

    public async Task<Playlist> GetPlaylistByIdAsync(int id)
    {
        return await _playlistRepository.GetByIdAsync(id);
    }

    public async Task<IList<Playlist>> GetAllPlaylistsAsync(string name = null)
    {
        var playlists = await _playlistRepository.GetAllAsync();

        if (!string.IsNullOrEmpty(name))
        {
            playlists = playlists.Where(p => p.Name.Contains(name));
        }

        return playlists.ToList();
    }

    public async Task InsertPlaylistAsync(Playlist playlist)
    {
        await _playlistRepository.AddAsync(playlist);
    }

    public async Task InsertPlaylistsAsync(IList<Playlist> playlists)
    {
        foreach (var playlist in playlists)
        {
            await InsertPlaylistAsync(playlist);
        }
    }

    public async Task UpdatePlaylistAsync(Playlist playlist)
    {
        await _playlistRepository.UpdateAsync(playlist);
    }

    public async Task DeletePlaylistAsync(int id)
    {
        await _playlistRepository.DeleteAsync(id);
    }
}