using ClothingStoreMVC.Domain.Entities.QuizAggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClothingStoreMVC.Infrastructure.EntityConfigurations
{
    public class UserAnswerConfiguration : IEntityTypeConfiguration<UserAnswer>
    {
        public void Configure(EntityTypeBuilder<UserAnswer> builder)
        {
            builder.HasKey(ua => ua.Id);

            builder.HasOne(ua => ua.User)
                   .WithMany()
                   .HasForeignKey(ua => ua.UserId);

            builder.HasOne(ua => ua.Question)
                   .WithMany()
                   .HasForeignKey(ua => ua.QuestionId);

            builder.HasOne(ua => ua.Answer)
                   .WithMany()
                   .HasForeignKey(ua => ua.AnserId);
        }
    }
}
