using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using music_api.Entities;

namespace music_api.Configurations;

public class PerformerConfiguration : IEntityTypeConfiguration<Performer>
{
    public void Configure(EntityTypeBuilder<Performer> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Name).IsRequired().HasMaxLength(150);
        builder.Property(p => p.Description).HasMaxLength(500);
        builder.Property(p => p.Country).HasMaxLength(100);
        builder.Property(p => p.CreatedAt).IsRequired();
        builder.HasMany(p => p.Songs)
               .WithOne(s => s.Performer)
               .HasForeignKey(s => s.PerformerId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
