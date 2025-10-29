using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizera.Domain
{
    public class ApplicationUser : IdentityUser
    {
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;



        #region ApplicationUser  1----M Course
        public virtual ICollection<Course> Courses {  get; set; } = new List<Course>();
        #endregion

        #region ApplicationUser  M----M Attempt
        public virtual ICollection<Attempt> Attempts  { get; set; } = new List<Attempt>();
        #endregion
    }

}
