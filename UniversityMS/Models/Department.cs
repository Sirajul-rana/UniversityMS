using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UniversityMS.Models
{
    [Table("Department")]
    public class Department
    {
        [Key]
        public int DepartmentId { get; set; }

        [Required(ErrorMessage = "Please enter department code")]
        [Display(Name = "Code")]
        [StringLength(7,MinimumLength = 2,ErrorMessage = "Code must be 2-7 characters long")]
        [RegularExpression(@"^[a-zA-Z0-9''-'\s]{1,40}$", ErrorMessage = "Invalid Input")]
        [Remote("IsDepartmentCodeTaken", "Department", ErrorMessage = "Department code already taken")]
        public string DepartmentCode { get; set; }


        [Display(Name = "Name")]
        [Required(ErrorMessage = "Please enter department name")]
        [RegularExpression(@"^[a-zA-Z''-'\s]{1,40}$", ErrorMessage = "Invalid Input")]
        [Remote("IsDepartmentNameTaken", "Department", ErrorMessage = "Department code aleary taken")]
        public string DepartmentName { get; set; }
        
    }
}