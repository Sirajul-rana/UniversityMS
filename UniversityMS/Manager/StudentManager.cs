using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UniversityMS.Gateway;
using UniversityMS.Models;

namespace UniversityMS.Manager
{
    public class StudentManager
    {
        StudentGateway studentGateway = new StudentGateway();
        //Sirajuls code starts here
        public List<Student> GetStudents()
        {
            return studentGateway.GetStudents();
        }

        public Student GetStudent(int studentId)
        {
            Student student = studentGateway.GetStudent(studentId);
            Department department = new Department();
            department.DepartmentName = studentGateway.GetDepartmentByStudentId(student.DepartmentId);

            student.Department = department;
            
            return student;
        }

        public List<Course> GetCourses(int studentId)
        {
            List<Course> courses = studentGateway.GetCourses(studentId);
            return courses;
        }
        //Sirajuls code ends here


        //Ujjwal code starts here 
        public string RegisterStudent(Student student)
        {
            if (studentGateway.RegisterStudent(student) >= 0)
            {
                return "Success";
            }
            return "Failed";
        }
        public string EnrollStudent(EnrollStudent enrollStudent)
        {
            if (studentGateway.EnrollStudent(enrollStudent) >= 0)
            {
                return "Success";
            }
            return "Failed";
        }
        public List<Course> GetCoursesByEnrollId(int studentId)
        {
            return studentGateway.GetCoursesByEnrollId(studentId);
        }
        public List<Course> GetCoursesByDepartmentlId(int studentId)
        {
            return studentGateway.GetCoursesByDepartmentlId(studentId);
        }
        public List<Grade> GetGradeList()
        {
            return studentGateway.GetGradeList();
        }
        public Student GetStudentById(int studentId)
        {
            Student student = studentGateway.GetStudentById(studentId);
            Department department = new Department();
            department.DepartmentName = studentGateway.GetDepartmentByStudentId(student.DepartmentId);

            student.Department = department;

            return student;
        }
        public string SaveStudentResult(StudentResult studentResult)
        {
            if (studentGateway.SaveStudentResult(studentResult) >= 0)
            {
                return "Success";
            }
            return "Failed";
        }
        public string UpdateStudentResult(StudentResult studentResult)
        {
            if (studentGateway.UpdateStudentResult(studentResult) >= 0)
            {
                return "Updated";
            }
            return "Failed";
        }
        public bool IsResultAlreadyExists(int courseId, int studentId)
        {
            return studentGateway.IsResultAlreadyExists(courseId, studentId);
        }
        public bool IsCourseEnrolled(int courseId,int studentId)
        {
            return studentGateway.IsCourseEnrolled(courseId,studentId);
        }
        public bool IsStudentEmailExists(string studentEmail)
        {
            return studentGateway.IsStudentEmailExists(studentEmail);
        }
        //Ujjwal code ends here 
    }
}