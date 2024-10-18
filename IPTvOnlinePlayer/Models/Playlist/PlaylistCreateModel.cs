using System.ComponentModel.DataAnnotations;

namespace IPTvOnlinePlayer.Models.Playlist;

public class PlaylistCreateModel
{
    [Required(ErrorMessage = "Name field is required")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Display Order field is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Display Order must be a positive number")]
    public int DisplayOrder { get; set; }
    
    public string? LogoUrl { get; set; }

    public bool UseUrl { get; set; }

    #region m3u file source

    public string? FileUrl { get; set; }
    
    //or
    
    public IFormFile? File { get; set; }

    #endregion

}