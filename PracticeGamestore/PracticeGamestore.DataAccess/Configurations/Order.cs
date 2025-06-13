using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PracticeGamestore.DataAccess.Constants;
using PracticeGamestore.DataAccess.Enums;

namespace PracticeGamestore.DataAccess.Configurations;

public class Order : IEntityTypeConfiguration<Entities.Order>
{
    public void Configure(EntityTypeBuilder<Entities.Order> builder)
    {
        builder.ToTable("orders");

        builder.Property(o => o.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.HasKey(o => o.Id);

        builder.Property(o => o.UserEmail)
            .HasColumnName("user_email")
            .HasMaxLength(ValidationConstants.StringLength.ShortMaximum)
            .IsRequired();

        builder.Property(o => o.Status)
            .HasColumnName("status")
            .HasConversion<int>()
            .HasDefaultValue(OrderStatus.Created)
            .IsRequired();

        builder.Property(o => o.Total)
            .HasColumnName("total")
            .HasColumnType("decimal(18,2)")
            .IsRequired();
    }
}