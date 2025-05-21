using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace PracticeGamestore.DataAccess.Configurations;

public class GamePlatform: IEntityTypeConfiguration<Entities.GamePlatform>
{
    public void Configure(EntityTypeBuilder<Entities.GamePlatform> builder)
    {
        builder.ToTable("game_platform");
        
        builder.Property(gg => gg.GameId)
            .HasColumnName("game_id")
            .IsRequired();

        builder.Property(gg => gg.PlatformId)
            .HasColumnName("platform_id")
            .IsRequired();
        
        builder.HasKey(gp => new { gp.GameId, gp.PlatformId });

        builder.HasOne(gp => gp.Game)
            .WithMany(g => g.GamePlatforms)
            .HasForeignKey(gp => gp.GameId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(gp => gp.Platform)
            .WithMany(p => p.GamePlatforms)
            .HasForeignKey(gp => gp.PlatformId)    
            .OnDelete(DeleteBehavior.Cascade);

    }
}