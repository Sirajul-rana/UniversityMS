using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UniversityMS.Models
{
    [Table("AllocatedClassroom")]
    public class AllocatedClassroom
    {
       
        [Key]
        public int AllocatedClassroomId { get; set; }

        [Required(ErrorMessage = "please select an option")]
        [Display(Name = "Department")]
        public int DepartmentId { get; set; }
        
        [Required(ErrorMessage = "please select an option")]
        [Display(Name = "Course")]

        public int CourseId { get; set; }

        [Required(ErrorMessage = "please select an option")]
        [Display(Name = "Room No.")]
        //[Remote(action: "IsRoomAvailable", controller: "Admin", AdditionalFields = "RoomId,DayId,FromTime,ToTime", ErrorMessage = "This room can not be assign again")]
        public int RoomId { get; set; }

        [Required(ErrorMessage = "please select an option")]
        [Display(Name = "Day")]
        public int DayId { get; set; }
        
        [Required(ErrorMessage = "please enter the class start time")]
        [Display(Name = "From")]
        public string FromTime { get; set; }

        [Required(ErrorMessage = "please enter the class end time")]
        [Display(Name = "To")]
        public string ToTime { get; set; }

        public Course Course { get; set; }
        public Room Room { get; set; }
        public Day Day { get; set; }
        public Department Department { get; set; }
        public List<object> Objects { get; set; }
    }
}