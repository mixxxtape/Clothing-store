using ClothingStoreMVC.Domain.Entities.ProductAggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClothingStoreMVC.Infrastructure.EntityConfigurations
{
    public class ProductSizeConfiguration : IEntityTypeConfiguration<ProductSize>
    {
        public void Configure(EntityTypeBuilder<ProductSize> builder)
        {
            builder.HasKey(ps => ps.Id);

            builder.HasOne(ps => ps.Product)
                   .WithMany(p => p.Sizes)
                   .HasForeignKey(ps => ps.ProductId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(ps => ps.Size)
                   .WithMany(s => s.ProductSizes)
                   .HasForeignKey(ps => ps.SizeId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.Property(ps => ps.Quantity).IsRequired();
        }
    }

}
