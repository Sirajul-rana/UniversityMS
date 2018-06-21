using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UniversityMS.Manager;
using UniversityMS.Models;

namespace UniversityMS.Controllers
{
    public class TeacherController : Controller
    {
       
        AdminManager admin = new AdminManager();
        //Soikats code starts here
        public ActionResult SaveTeacher()
        {
            ViewData["designations"] = new SelectList(admin.GetDesignations(), "DesignationId", "DesignationName");
            ViewData["departments"] = new SelectList(admin.GetAllDepartment(), "DepartmentId", "DepartmentCode");
            return View();
        }

        [HttpPost]
        public ActionResult SaveTeacher(Teacher teacher)
        {
            var message = admin.SaveTeacher(teacher);
            return Json(message,JsonRequestBehavior.AllowGet);
        }

        public JsonResult IsTeacherEmailTaken(string teacherEmail)
        {
            return Json(!admin.IsTeacherEmailTaken(teacherEmail),JsonRequestBehavior.AllowGet);
        }
        //Soikats code ends here
	}
}