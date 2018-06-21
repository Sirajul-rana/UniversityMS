using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using UniversityMS.Models;

namespace UniversityMS.Gateway
{
    public class DepartmentGateway
    {
        //Soikats code starts here
        public List<Department> GetAllDepartments()
        {
            string query = "SELECT * FROM Department";
            Gateway gateway = new Gateway(query);

            SqlDataReader reader = gateway.SqlCommand.ExecuteReader();
            List<Department> departments = new List<Department>();
            while (reader.Read())
            {
                Department department = new Department();
                department.DepartmentName = reader["DepartmentName"].ToString();
                department.DepartmentCode = reader["DepartmentCode"].ToString();

                departments.Add(department);
            }
            reader.Close();
            gateway.Connection.Close();
            return departments;
        }

        public string SaveDepartment(Department department)
        {
            string query = "INSERT INTO Department (DepartmentCode, DepartmentName) VALUES (@departmentCode, @departmentName)";
            Gateway gateway = new Gateway(query);
            gateway.SqlCommand.Parameters.Clear();
            gateway.SqlCommand.Parameters.AddWithValue("@departmentCode", department.DepartmentCode);
            gateway.SqlCommand.Parameters.AddWithValue("@departmentName", department.DepartmentName);

            int rowAffected = gateway.SqlCommand.ExecuteNonQuery();
            if (rowAffected >= 0)
            {
                return "Added successfully";
            }


            return "Somethings went wrong";
        }

        public bool IsDepartmentCodeTaken(string departmentCode)
        {
            string query = "SELECT TOP 1 DepartmentId FROM Department WHERE DepartmentCode= @departmentCode";

            Gateway gateway = new Gateway(query);
            gateway.SqlCommand.Parameters.Clear();

            gateway.SqlCommand.Parameters.AddWithValue("@departmentCode", departmentCode);

            SqlDataReader reader = gateway.SqlCommand.ExecuteReader();

            if (reader.HasRows)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsDepartmentNameTaken(string departmentName)
        {

            string query = "SELECT TOP 1 DepartmentId FROM Department WHERE DepartmentName= @departmentName";

            Gateway gateway = new Gateway(query);
            gateway.SqlCommand.Parameters.Clear();

            gateway.SqlCommand.Parameters.AddWithValue("@departmentName", departmentName);

            SqlDataReader reader = gateway.SqlCommand.ExecuteReader();

            if (reader.HasRows)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        //Soikats code ends here
    }
}