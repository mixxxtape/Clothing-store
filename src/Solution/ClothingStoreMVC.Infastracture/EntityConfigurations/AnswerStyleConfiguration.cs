using ClothingStoreMVC.Domain.Entities.QuizAggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClothingStoreMVC.Infrastructure.EntityConfigurations
{
    public class AnswerStyleConfiguration : IEntityTypeConfiguration<AnswerStyle>
    {
        public void Configure(EntityTypeBuilder<AnswerStyle> builder)
        {
            builder.HasKey(asl => asl.Id);

            builder.HasOne(asl => asl.Answer)
                   .WithMany(a => a.Styles)
                   .HasForeignKey(asl => asl.AnswerId)
                   .IsRequired();

            builder.HasOne(asl => asl.Style)
                   .WithMany()
                   .HasForeignKey(asl => asl.StyleId)
                   .IsRequired();

        }
    }
}
