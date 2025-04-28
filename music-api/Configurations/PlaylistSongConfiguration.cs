using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using music_api.Entities;

namespace music_api.Configurations;

public class PlaylistSongConfiguration : IEntityTypeConfiguration<PlaylistSong>
{
    public void Configure(EntityTypeBuilder<PlaylistSong> builder)
    {
        builder.HasKey(ps => new { ps.PlaylistId, ps.SongId });

        builder.HasOne(ps => ps.Playlist)
            .WithMany(p => p.PlaylistSongs)
            .HasForeignKey(ps => ps.PlaylistId);

        builder.HasOne(ps => ps.Song)
            .WithMany(s => s.PlaylistSongs)
            .HasForeignKey(ps => ps.SongId);
    }
}
