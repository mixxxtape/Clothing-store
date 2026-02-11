using ClothingStoreMVC.Domain.Entities.QuizAggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClothingStoreMVC.Infrastructure.EntityConfigurations
{
    public class QuizConfiguration : IEntityTypeConfiguration<Quiz>
    {
        public void Configure(EntityTypeBuilder<Quiz> builder)
        {
            builder.HasKey(q => q.Id);
            builder.Property(q => q.Name).IsRequired().HasMaxLength(100);
            builder.Property(q => q.Description).HasMaxLength(500);

            builder.HasMany(q => q.Questions)
                   .WithOne(q => q.Quiz) 
                   .HasForeignKey(q => q.QuizId);

        }
    }
}
