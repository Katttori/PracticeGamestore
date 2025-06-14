using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PracticeGamestore.DataAccess.Constants;
using PracticeGamestore.DataAccess.Enums;

namespace PracticeGamestore.DataAccess.Configurations;

public class Country : IEntityTypeConfiguration<Entities.Country>
{
    public void Configure(EntityTypeBuilder<Entities.Country> builder)
    {
        builder.ToTable("countries");

        builder.Property(c => c.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Name)
            .HasColumnName("name")
            .HasMaxLength(ValidationConstants.StringLength.ShortMaximum)
            .IsRequired();
        
        builder.HasIndex(c => c.Name).IsUnique();

        builder.Property(c => c.CountryStatus)
            .HasColumnName("status")
            .HasConversion<int>()
            .HasDefaultValue(CountryStatus.Allowed)
            .IsRequired();
    }
}