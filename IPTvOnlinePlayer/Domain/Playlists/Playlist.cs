namespace IPTvOnlinePlayer.Domain.Playlists;

public class Playlist : BaseEntity
{
    //Indicates friendly name of the playlist
    public string Name { get; set; }

    public int DisplayOrder { get; set; }
    
    public string LogoUrl { get; set; }

    public int MediaItemCount { get; set; }

    public int CategoryCount { get; set; }
}