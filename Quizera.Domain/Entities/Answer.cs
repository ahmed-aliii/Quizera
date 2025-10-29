using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizera.Domain
{
    public class Answer
    {
        public int Id { get; set; }
        public string? WrittenAnswer { get; set; } // for text-based answers
        public bool IsCorrect { get; set; }

        #region Answer M----1 Question
        public virtual Question Question { get; set; }

        [ForeignKey("Question")]
        public int QuestionId { get; set; }
        #endregion

        #region  Answer 1---1 Option
        public virtual Option? Option { get; set; }

        [ForeignKey("Option")]
        public int? OptionId { get; set; } // chosen option (nullable for written answers)
        #endregion

        #region MyRegion
        public virtual Attempt Attempt { get; set; }
       
        [ForeignKey("Attempt")]
        public int AttemptId { get; set; }
        #endregion


    }

}
