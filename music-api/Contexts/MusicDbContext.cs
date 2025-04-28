using Microsoft.EntityFrameworkCore;
using music_api.Configurations;
using music_api.Entities;

namespace music_api.Contexts;

public class MusicDbContext : DbContext
{
    public DbSet<Song> Songs { get; set; }
    public DbSet<Performer> Performers { get; set; }
    public DbSet<Genre> Genres { get; set; }
    public DbSet<Playlist> Playlists { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<PlaylistSong> PlaylistSongs { get; set; }

    public MusicDbContext(DbContextOptions<MusicDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new PlaylistConfiguration());
        modelBuilder.ApplyConfiguration(new SongConfiguration());
        modelBuilder.ApplyConfiguration(new PerformerConfiguration());
        modelBuilder.ApplyConfiguration(new GenreConfiguration());

        modelBuilder.ApplyConfiguration(new PlaylistSongConfiguration());

        base.OnModelCreating(modelBuilder);
    }
}