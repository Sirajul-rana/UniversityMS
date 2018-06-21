using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UniversityMS.Models
{
    [Table("Student")]
    public class Student
    {
        [Key]
        public int StudentId { get; set; }

        [Required(ErrorMessage = "Please enter your name")]
        [RegularExpression(@"^[a-zA-Z''-'\s]{1,40}$", ErrorMessage = "Invalid Input")]
        [Display(Name = "Name")]
        public string StudentName { get; set; }

        [Required(ErrorMessage = "Please enter your email")]
        [RegularExpression(@"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?", ErrorMessage = "Please enter a valid email")]
        [Display(Name = "Email")]
        [Remote("IsStudentEmailExists","Student",ErrorMessage = "Email already exists")]
        public string StudentEmail { get; set; }

        [Required(ErrorMessage = "Please enter your contact")]
        [Display(Name = "Contact No.")]
        [RegularExpression(@"\+?(88)?0?1[56789][0-9]{8}\b", ErrorMessage = "Please enter valid contact no(+88) or (01)")]
        public string StudentContactNo { get; set; }

        [Required(ErrorMessage = "Please enter your address")]
        [Display(Name = "Address")]
        public string StudentAddress { get; set; }

        [Required(ErrorMessage = "Please select an option")]
        [Display(Name = "Department")]
        public int DepartmentId { get; set; }

        [Display(Name = "Student Reg. No.")]
        public string RegistrationCode { get; set; }

        [Required(ErrorMessage = "Please select an date")]
        [Display(Name = "Date")]
        public DateTime RegisterDate { get; set; }
        public Department Department { get; set; }


    }
}