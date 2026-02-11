using ClothingStoreMVC.Domain.Entities.QuizAggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class ResultConfiguration : IEntityTypeConfiguration<Result>
{
    public void Configure(EntityTypeBuilder<Result> builder)
    {
        builder.HasKey(r => r.Id);

        builder.HasOne(r => r.User)
               .WithMany(u => u.Results)
               .HasForeignKey(r => r.UserId);

        builder.HasOne(r => r.Quiz)
               .WithMany(q => q.Results)
               .HasForeignKey(r => r.QuizId);

        builder.HasOne(r => r.Style)
               .WithMany()
               .HasForeignKey(r => r.StyleId);
    }
}
