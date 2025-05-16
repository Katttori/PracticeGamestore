using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace PracticeGamestore.DataAccess.Configurations;

public class GameOrder: IEntityTypeConfiguration<Entities.GameOrder>
{
    public void Configure(EntityTypeBuilder<Entities.GameOrder> builder)
    {
        builder.ToTable("game_order");
        
        builder.Property(go=> go.Game)
            .HasColumnName("game_id")
            .IsRequired();

        builder.Property(go => go.Order)
            .HasColumnName("order_id")
            .IsRequired();
        
        builder.HasKey(go => new { go.GameId, go.OrderId });

        builder.HasOne(go => go.Game)
            .WithMany(g => g.GameOrders)
            .HasForeignKey(go => go.GameId);

        builder.HasOne(go => go.Order)
            .WithMany(p => p.GameOrders)
            .HasForeignKey(go => go.OrderId);
    }
}