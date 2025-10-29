using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Quizera.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizera.DALS
{
    public class AnswerConfiguration : IEntityTypeConfiguration<Answer>
    {
        public void Configure(EntityTypeBuilder<Answer> builder)
        {
            builder.Property(a => a.WrittenAnswer)
                   .HasMaxLength(1000);

            builder.Property(a => a.IsCorrect)
                   .IsRequired();

            builder.HasOne(a => a.Question)
                   .WithMany(q => q.Answers)
                   .HasForeignKey(a => a.QuestionId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(a => a.Option)
                   .WithMany()
                   .HasForeignKey(a => a.OptionId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(a => a.Attempt)
                   .WithMany(at => at.Answers)
                   .HasForeignKey(a => a.AttemptId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
