using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

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
            .HasMaxLength(50)
            .IsRequired();
        
        builder.HasIndex(g => g.Name).IsUnique();

        builder.Property(g => g.Description)
            .HasColumnName("description")
            .HasMaxLength(255);

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
        var strategyId = Guid.NewGuid();
        var rpgId = Guid.NewGuid();
        var sportsId = Guid.NewGuid();
        var actionId = Guid.NewGuid();
        var puzzleSkillId = Guid.NewGuid();

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

