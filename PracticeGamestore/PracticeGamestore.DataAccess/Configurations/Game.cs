using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PracticeGamestore.DataAccess.Constants;

namespace PracticeGamestore.DataAccess.Configurations;

public class Game : IEntityTypeConfiguration<Entities.Game>
{
    public void Configure(EntityTypeBuilder<Entities.Game> builder)
    {
        builder.ToTable("games");

        builder.Property(g => g.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.HasKey(g => g.Id);

        builder.Property(g => g.Name)
            .HasColumnName("name")
            .HasMaxLength(ValidationConstants.StringLength.ShortMaximum)
            .IsRequired();
        
        builder.HasIndex(g => g.Name).IsUnique();

        builder.Property(g => g.Key)
            .HasColumnName("key")
            .HasMaxLength(ValidationConstants.StringLength.ShortMaximum)
            .IsRequired();
        
        builder.HasIndex(g => g.Key).IsUnique();

        builder.Property(g => g.Description)
            .HasColumnName("description")
            .HasMaxLength(ValidationConstants.StringLength.LongMaximum);

        builder.Property(g => g.Price)
            .HasColumnName("price")
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(g => g.Picture)
            .HasColumnName("picture")
            .HasMaxLength(ValidationConstants.MaximumPictureSize);

        builder.Property(g => g.Rating)
            .HasColumnName("rating")
            .IsRequired();

        builder.Property(g => g.AgeRating)
            .HasColumnName("age_rating")
            .HasConversion<int>()
            .IsRequired();

        builder.Property(g => g.ReleaseDate)
            .HasColumnName("release_date")
            .IsRequired();

        builder.Property(g => g.PublisherId)
            .HasColumnName("publisher_id")
            .IsRequired();

        builder.HasOne(g => g.Publisher)
            .WithMany(p => p.Games)
            .HasForeignKey(g => g.PublisherId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}