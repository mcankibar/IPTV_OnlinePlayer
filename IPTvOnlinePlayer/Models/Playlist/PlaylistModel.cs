namespace IPTvOnlinePlayer.Models.Playlist;

public class PlaylistModel : BaseModel
{
    public string Name { get; set; }
    
    public int DisplayOrder { get; set; }

    public string LogoUrl { get; set; }

    public int CategoryCount { get; set; }

    public int MediaItemCount { get; set; }
}