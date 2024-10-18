namespace IPTvOnlinePlayer.Models.MediaItem;

public class MediaItemModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string StreamUrl { get; set; }
    public string LogoUrl { get; set; }
    public bool Favorite { get; set; }

    public string CategoryName { get; set; }

    public string PlaylistName { get; set; }
}