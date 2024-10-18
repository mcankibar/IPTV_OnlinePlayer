namespace IPTvOnlinePlayer.Domain.Playlists;

public class MediaCategory : BaseEntity
{
    public string Name { get; set; }

    public int DisplayOrder { get; set; }

    public bool Published { get; set; }

    public int PlaylistId { get; set; }
    
    //ToDo: Keep it as picture in local storage or as a link to the picture
    public string LogoUrl { get; set; }
}