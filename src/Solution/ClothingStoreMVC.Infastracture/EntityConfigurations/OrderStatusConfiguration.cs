using ClothingStoreMVC.Domain.Entities.UserAggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClothingStoreMVC.Infrastructure.EntityConfigurations
{
    public class OrderStatusConfiguration : IEntityTypeConfiguration<OrderStatus>
    {
        public void Configure(EntityTypeBuilder<OrderStatus> builder)
        {
            builder.HasKey(os => os.Id);

            builder.Property(os => os.Status)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(os => os.ChangeReason)
                   .HasMaxLength(200);

            builder.Property(os => os.ChangedAt)
                   .IsRequired();

            builder.HasOne(os => os.Order)
                   .WithMany(o => o.StatusHistory)
                   .HasForeignKey(os => os.OrderId)
                   .IsRequired();
        }
    }
}
