namespace IPTvOnlinePlayer.Models.MediaCategory;

public class MediaCategoryListModel
{
    public string PlaylistName { get; set; }
    public IList<MediaCategoryModel> Categories  { get; set; } = new List<MediaCategoryModel>();
}