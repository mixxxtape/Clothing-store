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

            builder.Property(o => o.OrderDate)
                   .IsRequired();

            builder.Property(o => o.DeliveryAddress)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.HasOne(o => o.User)
                   .WithMany(u => u.Orders)
                   .HasForeignKey(o => o.UserId)
                   .IsRequired();

            builder.HasMany(o => o.Items)
                   .WithOne()
                   .HasForeignKey(oi => oi.OrderId)
                   .IsRequired();

            builder.HasMany(o => o.StatusHistory)
                   .WithOne(os => os.Order)
                   .HasForeignKey(os => os.OrderId)
                   .IsRequired();
        }
    }
}
