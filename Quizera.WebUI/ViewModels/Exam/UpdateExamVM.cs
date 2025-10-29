using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Quizera.WebUI
{
    public class UpdateExamVM
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "Exam title is required.")]
        [StringLength(150, ErrorMessage = "Title must be less than 150 characters.")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description is required.")]
        [StringLength(1000, ErrorMessage = "Description must be less than 1000 characters.")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Duration is required.")]
        [Range(1, 300, ErrorMessage = "Duration must be between 1 and 300 minutes.")]
        [Display(Name = "Duration (Minutes)")]
        public int DurationInMinutes { get; set; }

        [Required(ErrorMessage = "Please select a course.")]
        [Display(Name = "Course")]
        public int CourseId { get; set; }

        // Used for dropdown list
        public IEnumerable<SelectListItem>? Courses { get; set; }
    }
}
