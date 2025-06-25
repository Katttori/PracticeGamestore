using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PracticeGamestore.DataAccess.Constants;

namespace PracticeGamestore.DataAccess.Configurations;

public class Genre : IEntityTypeConfiguration<Entities.Genre>
{
    public void Configure(EntityTypeBuilder<Entities.Genre> builder)
    {
        builder.ToTable("genres");

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

        builder.Property(g => g.Description)
            .HasColumnName("description")
            .HasMaxLength(ValidationConstants.StringLength.LongMaximum);

        builder.Property(g => g.ParentId)
            .HasColumnName("parent_id");
       
        builder.HasOne(g => g.Parent)
            .WithMany(g => g.Children)
            .HasForeignKey(g => g.ParentId)
            .OnDelete(DeleteBehavior.NoAction);
        
        SeedGenres(builder);
    }
    
    
    private void SeedGenres(EntityTypeBuilder<Entities.Genre> builder)
    {
        var strategyId = Guid.Parse("11111111-1111-1111-1111-111111111111");
        var rpgId = Guid.Parse("22222222-2222-2222-2222-222222222222");
        var sportsId = Guid.Parse("33333333-3333-3333-3333-333333333333");
        var actionId = Guid.Parse("44444444-4444-4444-4444-444444444444");
        var puzzleSkillId = Guid.Parse("55555555-5555-5555-5555-555555555555");

        builder.HasData(
            new Entities.Genre { Id = strategyId, Name = "Strategy", Description = "Strategic thinking and planning games" },
            new Entities.Genre { Id = rpgId, Name = "RPG", Description = "Role-playing games" },
            new Entities.Genre { Id = sportsId, Name = "Sports", Description = "Sports simulation and arcade games" },
            new Entities.Genre { Id = actionId, Name = "Action", Description = "Fast-paced action games" },
            new Entities.Genre { Id = puzzleSkillId, Name = "Puzzle & Skill", Description = "Brain teasers and skill-based games" }
        );

        builder.HasData(
            new Entities.Genre { Id = Guid.NewGuid(), Name = "RTS", Description = "Real-time strategy", ParentId = strategyId },
            new Entities.Genre { Id = Guid.NewGuid(), Name = "TBS", Description = "Turn-based strategy", ParentId = strategyId }
        );

        builder.HasData(
            new Entities.Genre { Id = Guid.NewGuid(), Name = "Races", Description = "Racing games", ParentId = sportsId },
            new Entities.Genre { Id = Guid.NewGuid(), Name = "Rally", Description = "Rally racing", ParentId = sportsId },
            new Entities.Genre { Id = Guid.NewGuid(), Name = "Arcade", Description = "Arcade sports", ParentId = sportsId },
            new Entities.Genre { Id = Guid.NewGuid(), Name = "Formula", Description = "Formula racing", ParentId = sportsId },
            new Entities.Genre { Id = Guid.NewGuid(), Name = "Off-road", Description = "Off-road racing", ParentId = sportsId }
        );

        builder.HasData(
            new Entities.Genre { Id = Guid.NewGuid(), Name = "FPS", Description = "First-person shooter", ParentId = actionId },
            new Entities.Genre { Id = Guid.NewGuid(), Name = "TPS", Description = "Third-person shooter", ParentId = actionId },
            new Entities.Genre { Id = Guid.NewGuid(), Name = "Adventure", Description = "Action adventure games", ParentId = actionId }
        );
    }
}

