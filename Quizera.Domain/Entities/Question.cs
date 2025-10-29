using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizera.Domain
{
    public class Question
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int Marks { get; set; }

        #region Question M---1 Exam
        public virtual Exam Exam { get; set; }
        
        [ForeignKey("Exam")]
        public int ExamId { get; set; }
        #endregion

        #region Question 1---M Option
        public virtual ICollection<Option> Options { get; set; } = new List<Option>();
        #endregion

        #region Question 1---M Answer
        public ICollection<Answer> Answers { get; set; } = new List<Answer>(); // student answers

        #endregion


    }

}
