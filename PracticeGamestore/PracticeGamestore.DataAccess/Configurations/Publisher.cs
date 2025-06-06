using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PracticeGamestore.DataAccess.Constants;

namespace PracticeGamestore.DataAccess.Configurations;

public class Publisher : IEntityTypeConfiguration<Entities.Publisher>
{
    public void Configure(EntityTypeBuilder<Entities.Publisher> builder)
    {
        builder.ToTable("publishers");

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

        builder.Property(p => p.PageUrl)
            .HasColumnName("page_url")
            .HasMaxLength(ValidationConstants.StringLength.LongMaximum)
            .IsRequired();
        
        builder.HasIndex(p => p.PageUrl).IsUnique();
    }
}