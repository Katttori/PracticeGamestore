using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace PracticeGamestore.DataAccess.Configurations;

public class File: IEntityTypeConfiguration<Entities.File>
{
    public void Configure(EntityTypeBuilder<Entities.File> builder)
    {
        builder.ToTable("files");

        builder.Property(f => f.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.HasKey(f => f.Id);

        builder.Property(f => f.Type)
            .HasColumnName("type")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(f => f.Path)
            .HasColumnName("path")
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(f => f.CreationDate)
            .HasColumnName("creation_date")
            .HasDefaultValueSql("GETUTCDATE()")
            .IsRequired();

        builder.Property(f => f.Size)
            .HasColumnName("size")
            .IsRequired();

        builder.Property(f => f.GameId)
            .HasColumnName("game_id")
            .IsRequired();

        builder.HasOne(f => f.Game)
            .WithMany(g => g.Files)
            .HasForeignKey(f => f.GameId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}