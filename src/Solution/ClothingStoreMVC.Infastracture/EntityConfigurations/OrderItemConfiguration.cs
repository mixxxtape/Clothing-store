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

            builder.Property(oi => oi.Quantity).IsRequired();
            builder.Property(oi => oi.ProductName).IsRequired();
            builder.Property(oi => oi.Price).IsRequired();

            builder.HasOne(oi => oi.Product)
                   .WithMany()
                   .HasForeignKey(oi => oi.ProductId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(oi => oi.ProductSize)
                   .WithMany()
                   .HasForeignKey(oi => oi.ProductSizeId)
                   .OnDelete(DeleteBehavior.Restrict);

        }
    }

}

