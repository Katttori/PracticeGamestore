using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

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
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(p => p.Description)
            .HasColumnName("description")
            .HasMaxLength(255);

        builder.HasData(
            new Entities.Platform { Id = Guid.NewGuid(), Name = "Android" },
            new Entities.Platform { Id = Guid.NewGuid(), Name = "IOS" },
            new Entities.Platform { Id = Guid.NewGuid(), Name = "Windows" },
            new Entities.Platform { Id = Guid.NewGuid(), Name = "VR" }
        );
    }
}