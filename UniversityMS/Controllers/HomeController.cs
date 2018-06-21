using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UniversityMS.Manager;

namespace UniversityMS.Controllers
{
    public class HomeController : Controller
    {
        AdminManager Manager = new AdminManager();
        //Sirajuls code starts here
        public ActionResult Index()
        {
            ViewBag.students =Manager.GetTotalStudents();
            ViewBag.teachers =Manager.GetTotalTeachers();
            ViewBag.courses = Manager.GetTotalCourses();
            ViewBag.Departments = Manager.GetTotalDepartments();
            return View();
        }
        //Sirajuls code ends here
	}
}