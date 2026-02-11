using ClothingStoreMVC.Domain.Entities.ProductAggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Options;
using System;

namespace ClothingStoreMVC.Infrastructure.EntityConfigurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name).IsRequired().HasMaxLength(100);
            builder.Property(p => p.Description).HasMaxLength(500);
            builder.Property(p => p.Price).IsRequired();

            builder.HasQueryFilter(p => !p.IsDeleted);

            builder.HasOne(p => p.Category)
                   .WithMany(c => c.Products)
                   .HasForeignKey(p => p.CategoryId);

            builder.HasOne(p => p.Style)
                   .WithMany(s => s.Products)
                   .HasForeignKey(p => p.StyleId);

            builder.HasMany(p => p.Reviews)
                   .WithOne(r => r.Product)
                   .HasForeignKey(r => r.ProductId);

            builder.HasMany(p => p.Sizes)
                   .WithOne()
                   .HasForeignKey(ps => ps.ProductId);
        }

    }
}
