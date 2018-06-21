using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace UniversityMS.Models
{
    [Table("Semester")]
    public class Semester
    {
        [Key]
        public int SemesterId { get; set; }

        public string SemesterName { get; set; }
    }
}