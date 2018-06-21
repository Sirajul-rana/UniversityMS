using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UniversityMS.Manager;
using UniversityMS.Models;

namespace UniversityMS.Controllers
{
    public class CourseController : Controller
    {
        //Soikats code starts here 
        AdminManager admin = new AdminManager();
       
        [HttpGet]
        public ActionResult SaveCourse()
        {
            ViewData["departments"] = new SelectList(admin.GetAllDepartment(), "DepartmentId", "DepartmentCode");
            ViewData["semesters"] = new SelectList(admin.GetSemesters(), "SemesterId", "SemesterName");
            return View();
        }

        [HttpPost]
        public ActionResult SaveCourse(Course course)
        {
            var message = admin.SaveCourse(course);
            return Json(message,JsonRequestBehavior.AllowGet);
        }


        public JsonResult IsCourseCodeTaken(string courseCode)
        {
            return Json(!admin.IsCourseCodeTaken(courseCode), JsonRequestBehavior.AllowGet);
        }

        public JsonResult IsCourseNameTaken(string courseName)
        {
            return Json(!admin.IsCourseNameTaken(courseName), JsonRequestBehavior.AllowGet);
        }
        //Soikats code ends here
	}
}