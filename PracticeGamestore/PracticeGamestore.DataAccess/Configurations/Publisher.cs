using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

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
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(p => p.Description)
            .HasColumnName("description")
            .HasMaxLength(255);

        builder.Property(p => p.PageUrl)
            .HasColumnName("page_url")
            .HasMaxLength(500)
            .IsRequired();
    }
}