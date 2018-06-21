using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace UniversityMS.Models
{
    [Table("Day")]
    public class Day
    {
        [Key]
        public int DayId { get; set; }
        public string DayName { get; set; }
    }
}