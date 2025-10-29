using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizera.Domain
{
    public class Option
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public bool IsCorrect { get; set; }

      
        #region Option 1----M Answer
        public virtual Question Question { get; set; }

        [ForeignKey("Question")]
        public int QuestionId { get; set; }
        #endregion

    }

}
