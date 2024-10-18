using IPTvOnlinePlayer.Domain.Playlists;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IPTvOnlinePlayer.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public DbSet<MediaCategory> MediaCategories { get; set; }
    public DbSet<MediaItem> MediaItems { get; set; }
    public DbSet<Playlist> Playlists { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
}