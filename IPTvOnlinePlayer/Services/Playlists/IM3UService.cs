using m3uParser;
using m3uParser.Model;

namespace IPTvOnlinePlayer.Services.Playlists;

public interface IM3UService
{
    public Extm3u ProcessM3U(IFormFile file);

    public Extm3u ProcessM3U(string fileUrl);
}

public class M3UService : IM3UService
{   
    public Extm3u ProcessM3U(IFormFile file)
    {
        using var stream = file.OpenReadStream();
        using var reader = new StreamReader(stream);
        var content = reader.ReadToEnd();
        
        return M3U.Parse(content);
    }
    
    public Extm3u ProcessM3U(string fileUrl)
    {
        using var client = new HttpClient();
        var content = client.GetStringAsync(fileUrl).Result;
        
        return M3U.Parse(content);
    }
}