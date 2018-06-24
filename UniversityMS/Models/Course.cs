using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UniversityMS.Models
{
    [Table("Course")]
    public class Course
    {
        [Key]
        public int CourseId { get; set; }

        [Required(ErrorMessage = "Please enter course name")]
        [RegularExpression(@"^[a-zA-Z0-9'\s]{1,40}$", ErrorMessage = "Invalid Input")]
        [Display(Name = "Course Name")]
        [Remote("IsCourseNameTaken", "Course", ErrorMessage = "Course name already taken")]
        public string CourseName { get; set; }

        [Required(ErrorMessage = "Please enter course code")]
        [Display(Name = "Course code")]
        [StringLength(100,MinimumLength = 5,ErrorMessage = "Code must be at least 5 characters long")]
        [Remote("IsCourseCodeTaken", "Course", ErrorMessage = "Course code already taken")]
        public string CourseCode { get; set; }

        [Required(ErrorMessage = "Please enter credit")]
        [Range(0.5,5.0, ErrorMessage = "Credit must be 0.5-5.0")]
        [Display(Name = "Credit")]
        public double Credit { get; set; }

        [Required(ErrorMessage = "Please enter description of the course")]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Please select an option")]
        [Display(Name = "Department")]
        public int DepartmentId { get; set; }
        [Required(ErrorMessage = "Please select an option")]
        public int SemesterId { get; set; }

        public string Schedule { get; set; }
        public Grade Grade { get; set; }
        public Semester Semester { get; set; }
    }
}