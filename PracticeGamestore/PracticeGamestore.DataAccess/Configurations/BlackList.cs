using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PracticeGamestore.DataAccess.Constants;
using PracticeGamestore.DataAccess.Entities;

namespace PracticeGamestore.DataAccess.Configurations;

public class BlackList : IEntityTypeConfiguration<Blacklist>
{
    public void Configure(EntityTypeBuilder<Blacklist> builder)
    {
        builder.ToTable("blacklists");

        builder.Property(b => b.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd()
            .IsRequired();
        
        builder.HasKey(b => b.Id);

        builder.Property(b => b.CountryId)
            .HasColumnName("country_id")
            .IsRequired();

        builder.Property(b => b.UserEmail)
            .HasColumnName("user_email")
            .HasMaxLength(ValidationConstants.StringLength.ShortMaximum)
            .IsRequired();
        
        builder.HasIndex(b => b.UserEmail).IsUnique();

        builder.HasOne(b => b.Country)
            .WithMany(c => c.Blacklists)
            .HasForeignKey(b => b.CountryId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}