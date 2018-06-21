using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace UniversityMS.Models
{
    [Table("StudentResult")]
    public class StudentResult
    {
        [Key]
        public int StudentResultId { get; set; }

        [Required(ErrorMessage = "Please select an option")]
        [Display(Name = "Student Reg No")]
        public int StudentRegId { get; set; }

        [Required(ErrorMessage = "Please select an option")]
        [Display(Name = "Select Course")]
        public int CourseId { get; set; }

        [Required(ErrorMessage = "Please select an option")]
        [Display(Name = "Select Grade Letter")]
        public int GradeId { get; set; }
        public Student Student { get; set; }
        public Course Course { get; set; }
        public Grade Grade { get; set; }
    }
}