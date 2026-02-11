using ClothingStoreMVC.Domain.Entities.UserAggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClothingStoreMVC.Infrastructure.EntityConfigurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);

            builder.Property(u => u.Name).IsRequired().HasMaxLength(50);
            builder.Property(u => u.Surname).IsRequired().HasMaxLength(50);
            builder.Property(u => u.Email).IsRequired().HasMaxLength(100);
            builder.Property(u => u.Password).IsRequired().HasMaxLength(100);

            builder.HasOne(u => u.Cart)
                   .WithOne(c => c.User)
                   .HasForeignKey<Cart>(c => c.UserId);

            builder.HasOne(u => u.Wishlist)
                   .WithOne(w => w.User)
                   .HasForeignKey<Wishlist>(w => w.UserId);

            builder.HasMany(u => u.Orders)
                   .WithOne(o => o.User)
                   .HasForeignKey(o => o.UserId);

            builder.HasOne(u => u.Role)
                   .WithMany(r => r.Users)
                   .HasForeignKey(u => u.RoleId)
                   .IsRequired();
        }
    }
}
