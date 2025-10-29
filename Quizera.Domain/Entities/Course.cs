using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizera.Domain
{
    public class Course
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


        #region Course M----1 ApplicationUser  
        public virtual ApplicationUser Instructor { get; set; }

        [ForeignKey("Instructor")]
        public string InstructorId { get; set; }
        #endregion

        #region Course 1----M Exam
        public virtual ICollection<Exam> Exams { get; set; }
        #endregion
    }
}
