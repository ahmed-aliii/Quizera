using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizera.Domain
{
    public class Exam
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int DurationInMinutes { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        #region Exam M----1 Course
        public virtual Course Course { get; set; }

        public int CourseId { get; set; }
        #endregion

        #region Exam 1---M Question
        public virtual ICollection<Question> Questions { get; set; } = new List<Question>();
        #endregion

        #region Exam 1---M Attempts
        public virtual ICollection<Attempt> Attempts { get; set; } = new List<Attempt>();
        #endregion
    }

}
