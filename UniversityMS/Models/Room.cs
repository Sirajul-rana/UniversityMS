using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace UniversityMS.Models
{
    [Table("Room")]
    public class Room
    {
        [Key]
        public int RoomId { get; set; }

        public string RoomNo { get; set; }
    }
}