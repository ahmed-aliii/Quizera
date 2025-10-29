using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizera.Domain
{
    public class Attempt
    {
        public int Id { get; set; }
        public DateTime StartedAt { get; set; } = DateTime.UtcNow;
        public DateTime? FinishedAt { get; set; }
        public double Score { get; set; }

        #region Attempt M---1 Student
        public virtual ApplicationUser Student { get; set; }

        [ForeignKey("Student")]
        public string UserId { get; set; }
        #endregion

        #region Attempt  M----1 Exam
        public virtual Exam Exam { get; set; }

        [ForeignKey("Exam")]
        public int ExamId { get; set; }
        #endregion

        #region Attempt 1---M Answer
        public virtual ICollection<Answer> Answers { get; set; } = new List<Answer>();
        #endregion
    }

}
