namespace IPTvOnlinePlayer.Models.MediaCategory;

public class MediaCategoryModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    
    public int DisplayOrder { get; set; }
    
    public bool Published { get; set; }
    
    public string LogoUrl { get; set; }

    public int MediaCount { get; set; }
}