namespace IPTvOnlinePlayer.Domain.Playlists;

public class MediaItem : BaseEntity
{
    public string Name { get; set; }
    public string StreamUrl { get; set; }
    public string LogoUrl { get; set; }
    public bool Published { get; set; }
    public int DisplayOrder { get; set; }
    public int CategoryId { get; set; }

    public bool Favorite { get; set; }
}