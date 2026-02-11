using ClothingStoreMVC.Domain.Entities.UserAggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClothingStoreMVC.Infrastructure.EntityConfigurations
{
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.HasKey(oi => oi.Id);

            builder.Property(oi => oi.Quantity)
                   .IsRequired();

            // Зв'язок із Product
            builder.HasOne(oi => oi.Product)
                   .WithMany()
                   .HasForeignKey(oi => oi.ProductId)
                   .IsRequired();

            // Зв'язок із Size
            builder.HasOne(oi => oi.Size)
                   .WithMany()
                   .HasForeignKey(oi => oi.SizeId)
                   .IsRequired();
        }
    }
}
