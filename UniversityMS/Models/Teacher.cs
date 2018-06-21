using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UniversityMS.Models
{
    [Table("Teacher")]
    public class Teacher
    {
        [Key]
        public int TeacherId { get; set; }

        [Required(ErrorMessage = "Please enter your name")]
        [RegularExpression(@"^[a-zA-Z''-'\s]{1,40}$", ErrorMessage = "Invalid Input")]
        [Display(Name = "Name")]
        public string TeacherName { get; set; }

        [Required(ErrorMessage = "Please enter your address")]
        [Display(Name = "Address")]
        public string TeacherAddress { get; set; }

        [Required(ErrorMessage = "Please enter your email")]
        [Display(Name = "Email")]
        [RegularExpression(@"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?", ErrorMessage = "Please enter a valid email")]
        [Remote("IsTeacherEmailTaken", "Teacher", ErrorMessage = "This email already taken")]
        public string TeacherEmail { get; set; }

        [Required(ErrorMessage = "Please enter your contact no")]
        [RegularExpression(@"\+?(88)?0?1[56789][0-9]{8}\b", ErrorMessage = "Please enter valid contact no(+88) or (01)")]
        [Display(Name = "Contact No.")]
        public string TeacherContactNo { get; set; }

        [Required(ErrorMessage = "Please select an option")]
        [Display(Name = "Designation")]
        public int DesignationId { get; set; }

        [Required(ErrorMessage = "Please select an option")]
        [Display(Name = "Department")]
        public int DepartmentId { get; set; }

        [Required(ErrorMessage = "Please enter how much credit you want to take")]
        [Display(Name = "Credit to be taken")]
        [Range(0, 100, ErrorMessage = "Credit must be greater than 0")]
        public double TakenCredit { get; set; }
        public double RemainingCredit { get; set; }
        public Designation Designation { get; set; }
        public Department Department { get; set; }

    }
}