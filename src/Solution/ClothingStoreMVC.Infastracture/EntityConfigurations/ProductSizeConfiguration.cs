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

            builder.HasOne<Product>()
                   .WithMany(p => p.Sizes)
                   .HasForeignKey(ps => ps.ProductId);
        }
    }
}
