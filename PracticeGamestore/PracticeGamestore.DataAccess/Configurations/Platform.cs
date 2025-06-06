using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PracticeGamestore.DataAccess.Constants;

namespace PracticeGamestore.DataAccess.Configurations;

public class Platform : IEntityTypeConfiguration<Entities.Platform>
{
    public void Configure(EntityTypeBuilder<Entities.Platform> builder)
    {
        builder.ToTable("platforms");

        builder.Property(p => p.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name)
            .HasColumnName("name")
            .HasMaxLength(ValidationConstants.StringLength.ShortMaximum)
            .IsRequired();
        
        builder.HasIndex(p => p.Name).IsUnique();

        builder.Property(p => p.Description)
            .HasColumnName("description")
            .HasMaxLength(ValidationConstants.StringLength.LongMaximum);

        builder.HasData(
            new Entities.Platform { Id = Guid.NewGuid(), Name = "Android" },
            new Entities.Platform { Id = Guid.NewGuid(), Name = "IOS" },
            new Entities.Platform { Id = Guid.NewGuid(), Name = "Windows" },
            new Entities.Platform { Id = Guid.NewGuid(), Name = "VR" }
        );
    }
}