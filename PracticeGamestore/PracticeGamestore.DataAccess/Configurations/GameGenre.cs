using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PracticeGamestore.DataAccess.Configurations;

public class GameGenre : IEntityTypeConfiguration<Entities.GameGenre>
{
    public void Configure(EntityTypeBuilder<Entities.GameGenre> builder)
    {
        builder.ToTable("game_genre");

        builder.Property(gg => gg.GameId)
            .HasColumnName("game_id")
            .IsRequired();

        builder.Property(gg => gg.GenreId)
            .HasColumnName("genre_id")
            .IsRequired();

        builder.HasKey(gg => new { gg.GameId, gg.GenreId });

        builder.HasOne(gg => gg.Game)
            .WithMany(g => g.GameGenres)
            .HasForeignKey(gg => gg.GameId);

        builder.HasOne(gg => gg.Genre)
            .WithMany(g => g.GameGenres)
            .HasForeignKey(gg => gg.GenreId);
    }
}