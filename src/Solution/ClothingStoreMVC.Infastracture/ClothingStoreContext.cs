using Microsoft.EntityFrameworkCore;
using ClothingStoreMVC.Infrastructure.EntityConfigurations;

namespace ClothingStoreMVC.Infrastructure
{
    public class ClothingStoreContext : DbContext
    {
        public ClothingStoreContext(DbContextOptions<ClothingStoreContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
        }
    }
}
