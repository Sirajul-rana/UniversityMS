using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UniversityMS.Manager;
using UniversityMS.Models;

namespace UniversityMS.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly DepartmentManager departmentManager = new DepartmentManager();
        //Soikats code starts here
        public ActionResult Index()
        {
            return View(departmentManager.GetAllDepartments());
        }

        [HttpGet]
        public ActionResult SaveDepartment()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SaveDepartment(Department department)
        {
            var message = departmentManager.SaveDepartment(department);
            return Json(message);
        }


        public JsonResult IsDepartmentCodeTaken(string departmentCode)
        {
            return Json(!departmentManager.IsDepartmentCodeTaken(departmentCode),JsonRequestBehavior.AllowGet);
        }

        public JsonResult IsDepartmentNameTaken(string departmentName)
        {
            return Json(!departmentManager.IsDepartmentNameTaken(departmentName), JsonRequestBehavior.AllowGet);
        }
        //Soikats code ends here
	}
}