using IPTvOnlinePlayer.Models.MediaItem;

namespace IPTvOnlinePlayer.Models.Playlist;

public class PlaylistListModel
{
    public IList<PlaylistModel> Playlists { get; set; } = new List<PlaylistModel>();


    public IList<MediaItemModel> FavoriteMediaItems { get; set; } = new List<MediaItemModel>();
}