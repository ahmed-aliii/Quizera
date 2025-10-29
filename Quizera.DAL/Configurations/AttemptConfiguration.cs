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
    public class AttemptConfiguration : IEntityTypeConfiguration<Attempt>
    {
        public void Configure(EntityTypeBuilder<Attempt> builder)
        {
            builder.Property(a => a.StartedAt)
                   .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(a => a.Score)
                   .HasDefaultValue(0);

            builder.HasOne(a => a.Student)
                   .WithMany(u => u.Attempts)
                   .HasForeignKey(a => a.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(a => a.Exam)
                   .WithMany(e => e.Attempts)
                   .HasForeignKey(a => a.ExamId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
