using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace UniversityMS.Models
{
    [Table("Grade")]
    public class Grade
    {
        [Key]
        public int GradeId { get; set; }

        public string GradeCode { get; set; }
    }
}