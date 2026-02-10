using ClothingStoreMVC.Domain.Entities.UserAggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClothingStoreMVC.Infrastructure.EntityConfigurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(o => o.Id);

            builder.HasMany(o => o.Items)
                   .WithOne()
                   .HasForeignKey(oi => oi.Id);

            builder.HasMany(o => o.StatusHistory)
                   .WithOne(os => os.Order)
                   .HasForeignKey(os => os.OrderId);
        }
    }
}
