using Microsoft.EntityFrameworkCore;
using music_api.Entities;

namespace music_api.Contexts;

public class MusicDbContext : DbContext
{
    public DbSet<Song> Songs { get; set; }
    public DbSet<Performer> Performers { get; set; }
    public DbSet<Genre> Genres { get; set; }
    public DbSet<Playlist> Playlists { get; set; }
    public DbSet<User> Users { get; set; }

    public MusicDbContext(DbContextOptions<MusicDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new Configurations.UserConfiguration());
        modelBuilder.ApplyConfiguration(new Configurations.PlaylistConfiguration());
        modelBuilder.ApplyConfiguration(new Configurations.SongConfiguration());
        modelBuilder.ApplyConfiguration(new Configurations.PerformerConfiguration());
        modelBuilder.ApplyConfiguration(new Configurations.GenreConfiguration());
        base.OnModelCreating(modelBuilder);
    }
}