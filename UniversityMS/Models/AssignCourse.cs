using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

namespace UniversityMS.Models
{
    [Table("AssignCourse")]
    public class AssignCourse
    {
        [Key]
        public int AssignCourseId { get; set; }

        [Required(ErrorMessage = "please select an option")]
        [Display(Name = "Teacher")]
        public int TeacherId { get; set; }

        [Required(ErrorMessage = "please select an option")]
        [Display(Name = "Course")]
        [Remote("IsCourseTaken","Admin",ErrorMessage = "Course has been assigned")]
        public int CourseId { get; set; }

        [Display(Name = "Credit to be taken")]
        public int TakenCourseCredit { get; set; }

        [Required(ErrorMessage = "please select an option")]
        [Display(Name = "Department")]
        public int DepartmentId { get; set; }

        public Teacher Teacher { get; set; }
        public Course Course { get; set; }
        public Semester Semester { get; set; }

    }
}