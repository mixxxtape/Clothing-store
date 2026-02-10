using ClothingStoreMVC.Domain.Entities.QuizAggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClothingStoreMVC.Infrastructure.EntityConfigurations
{
    public class QuestionConfiguration : IEntityTypeConfiguration<Question>
    {
        public void Configure(EntityTypeBuilder<Question> builder)
        {
            builder.HasKey(q => q.Id);
            builder.Property(q => q.Text).IsRequired().HasMaxLength(500);

            builder.HasMany(q => q.Answers)
                   .WithOne()
                   .HasForeignKey("QuestionId");
        }
    }
}
