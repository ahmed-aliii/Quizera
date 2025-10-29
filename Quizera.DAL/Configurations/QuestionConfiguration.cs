using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Quizera.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizera.DAL.Configurations
{
    public class QuestionConfiguration : IEntityTypeConfiguration<Question>
    {
        public void Configure(EntityTypeBuilder<Question> builder)
        {
            builder.Property(q => q.Text)
                   .HasMaxLength(1000)
                   .IsRequired();

            builder.Property(q => q.Marks)
                   .HasDefaultValue(1);

            builder.HasOne(q => q.Exam)
                   .WithMany(e => e.Questions)
                   .HasForeignKey(q => q.ExamId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
