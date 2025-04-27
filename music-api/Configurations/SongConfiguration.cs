using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using music_api.Entities;

namespace music_api.Configurations;

public class SongConfiguration : IEntityTypeConfiguration<Song>
{
    public void Configure(EntityTypeBuilder<Song> builder)
    {
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Title).IsRequired().HasMaxLength(200);
        builder.Property(s => s.Duration).IsRequired();
        builder.Property(s => s.CreatedAt).IsRequired();
        builder.HasOne(s => s.Performer)
               .WithMany(p => p.Songs)
               .HasForeignKey(s => s.PerformerId)
               .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(s => s.Genre)
               .WithMany(g => g.Songs)
               .HasForeignKey(s => s.GenreId)
               .OnDelete(DeleteBehavior.SetNull);
    }
}
