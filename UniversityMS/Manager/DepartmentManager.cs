using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UniversityMS.Gateway;
using UniversityMS.Models;

namespace UniversityMS.Manager
{
    public class DepartmentManager
    {
        //Soikats code starts here
        private DepartmentGateway departmentGateway = new DepartmentGateway();
        public List<Department> GetAllDepartments()
        {
            return departmentGateway.GetAllDepartments();
        }

        public string SaveDepartment(Department department)
        {
            return departmentGateway.SaveDepartment(department);
        }

        public bool IsDepartmentCodeTaken(string departmentCode)
        {
            return departmentGateway.IsDepartmentCodeTaken(departmentCode);
        }

        public bool IsDepartmentNameTaken(string departmentName)
        {
            return departmentGateway.IsDepartmentNameTaken(departmentName);
        }
        //Soikats code ends here
    }
}