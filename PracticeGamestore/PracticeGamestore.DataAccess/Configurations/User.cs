using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PracticeGamestore.DataAccess.Enums;

namespace PracticeGamestore.DataAccess.Configurations;

public class User : IEntityTypeConfiguration<Entities.User>
{
    public void Configure(EntityTypeBuilder<Entities.User> builder)
    {
        builder.ToTable("users");
        
        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd()
            .IsRequired();
        
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.UserName)
            .HasColumnName("username")
            .HasMaxLength(100)
            .IsRequired();
        
        builder.Property(x => x.Email)
            .HasColumnName("email")
            .HasMaxLength(100)
            .IsRequired();
        
        builder.Property(x => x.PhoneNumber)
            .HasColumnName("phone_number")
            .HasMaxLength(15)
            .IsRequired();
        
        builder.Property(x => x.PasswordHash)
            .HasColumnName("password_hash")
            .HasMaxLength(255)
            .IsRequired();
        
        builder.Property(x => x.Role)
            .HasColumnName("role")
            .HasMaxLength(50)
            .IsRequired()
            .HasDefaultValue("User"); // Default role is User
        
        builder.Property(x => x.Status)
            .HasColumnName("status")
            .HasConversion<int>()
            .IsRequired()
            .HasDefaultValue(UserStatus.Active);
        
        builder.Property(x => x.CountryId)
            .HasColumnName("country_id")
            .IsRequired();
        
        builder.Property(x => x.BirthDate)
            .HasColumnName("birth_date")
            .IsRequired();
        
        builder.HasOne(x => x.Country)
            .WithMany()
            .HasForeignKey(x => x.CountryId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}