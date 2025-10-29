using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Quizera.WebUI
{
    public class CreateCourseVM
    {
        [Required(ErrorMessage = "Course title is required.")]
        [StringLength(100, ErrorMessage = "Title must be less than 100 characters.")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Course description is required.")]
        [StringLength(1000, ErrorMessage = "Description must be less than 1000 characters.")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Instructor is required.")]
        public string InstructorId { get; set; } = string.Empty;

        // Used to populate dropdown list of instructors in the view
        public IEnumerable<SelectListItem>? Instructors { get; set; }
    }
}
