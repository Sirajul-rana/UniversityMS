using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using UniversityMS.Manager;
using UniversityMS.Models;

namespace UniversityMS.Controllers
{
    public class AdminController : Controller
    {
        private AdminManager admin = new AdminManager();
        //Sirajuls code starts here
        [HttpGet]
        public ActionResult AssignCourseToTeacher()
        {
            ViewData["departments"] = new SelectList(admin.GetAllDepartment(), "DepartmentId", "DepartmentCode");
            return View();
        }

        [HttpPost]
        public JsonResult AssignCourseToTeacher(AssignCourse assignCourse)
        {
            string message = admin.AssignCourseToTeacher(assignCourse);
            return Json(message, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ViewCourseAssign()
        {
            ViewData["departments"] = new SelectList(admin.GetAllDepartment(), "DepartmentId", "DepartmentCode");
            return View();
        }

        [HttpPost]
        public ActionResult ViewCourseAssign(int departmentId)
        {
            List<AssignCourse> assignCourses = admin.GetCoursesInformation(departmentId);
            return Json(assignCourses);
        }

        [HttpGet]
        public ActionResult AllocateClassroom()
        {
            ViewData["departments"] = new SelectList(admin.GetAllDepartment(), "DepartmentId", "DepartmentCode");
            ViewData["rooms"] = new SelectList(admin.GetAllRooms(), "RoomId", "RoomNo");
            ViewData["days"] = new SelectList(admin.GetAllDays(), "DayId", "DayName");
            return View();
        }

        [HttpPost]
        public ActionResult AllocateClassroom(AllocatedClassroom allocatedClassroom)
        {
            var message = admin.AllocateClassroom(allocatedClassroom);
            return Json(message,JsonRequestBehavior.AllowGet);
        }



        public ActionResult UpdateTeacherTakenCredit(double extraCredit, int teacherId)
        {
            var message = admin.UpdateTeacherTakenCredit(extraCredit,teacherId);
            return Json(message, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetTeacherByDepartment(int departmentId)
        {
            List<Teacher> teachers = admin.GetTeacherByDepartment(departmentId);
            SelectList teacherSelectList = new SelectList(teachers, "TeacherId", "TeacherName", 0);
            return Json(teacherSelectList, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetCourseByDepartment(int departmentId)
        {
            List<Course> courses = admin.GetCourseByDepartment(departmentId);
            SelectList coursSelectList = new SelectList(courses, "CourseId", "CourseCode", 0);
            return Json(coursSelectList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetTeacherById(int teacherId)
        {
            Teacher teacher = admin.GetTeacherById(teacherId);
            return Json(teacher, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCourseById(int courseId)
        {
            Course course = admin.GetCourseById(courseId);
            return Json(course, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ViewSchedule()
        {
            ViewData["departments"] = new SelectList(admin.GetAllDepartment(), "DepartmentId", "DepartmentCode");
            return View();
        }

        [HttpPost]
        public ActionResult ViewSchedule(int departmentId)
        {
            List<Course> courses = admin.GetClassroomInformation(departmentId);
            return Json(courses,JsonRequestBehavior.AllowGet);
        }
        public JsonResult IsCourseTaken(int courseId)
        {
            return Json(!admin.IsCourseTaken(courseId), JsonRequestBehavior.AllowGet);
        }

        //public JsonResult IsRoomAvailable(int roomId, int dayId, string fromTime, string toTime)
        //{
        //    return Json(!admin.IsRoomAvailable(roomId,dayId,fromTime,toTime), JsonRequestBehavior.AllowGet);
        //}


        [HttpGet]
        public ActionResult UnassignCourses()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UnassignCourses(int code)
        {
            string message = admin.UnassignCourses(code);
            return Json(message,JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult UnallocateClassroom()
        {
            return View();
        }
        [HttpPost]
        public ActionResult UnallocateClassroom(int code)
        {
            string message = admin.UnallocateClassroom(code);
            return Json(message, JsonRequestBehavior.AllowGet);
        }
        //Sirajuls code starts here
	}
}