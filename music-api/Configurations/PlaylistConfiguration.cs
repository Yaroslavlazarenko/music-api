using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using music_api.Entities;

namespace music_api.Configurations;

public class PlaylistConfiguration : IEntityTypeConfiguration<Playlist>
{
    public void Configure(EntityTypeBuilder<Playlist> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Title).IsRequired().HasMaxLength(150);
        builder.Property(p => p.Description).HasMaxLength(500);
        builder.Property(p => p.CreatedAt).IsRequired();
        builder.HasMany(p => p.PlaylistSongs)
               .WithOne()
               .OnDelete(DeleteBehavior.Cascade);
    }
}
