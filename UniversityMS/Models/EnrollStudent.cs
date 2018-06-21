using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UniversityMS.Models
{
    [Table("EnrollStudent")]
    public class EnrollStudent
    {
        [Key]
        public int EnrollStudentId { get; set; }

        [Required(ErrorMessage = "Please select an option")]
        [Display(Name = "Student Reg No")]
        public int StudentRegEId { get; set; }

        [Required(ErrorMessage = "Please select an option")]
        [Display(Name = "Select Course")]
        [Remote("IsCourseEnrolled","Student",AdditionalFields = "StudentRegEId",ErrorMessage = "Course is already enrolled")]
        public int CourseId { get; set; }

        [Required(ErrorMessage = "Please enter a date")]
        [Display(Name = "Date")]
        public DateTime EnrollTime { get; set; }
    }
}