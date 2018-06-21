using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UniversityMS.Gateway;
using UniversityMS.Models;

namespace UniversityMS.Manager
{
    public class AdminManager
    {
        private AdminGateway admin = new AdminGateway();
        //Sirajuls code starts here
        public List<Department> GetAllDepartment()
        {
            return admin.GetAllDepartment();
        }

        public List<Teacher> GetTeacherByDepartment(int departmentId)
        {
            return admin.GetTeacherByDepartment(departmentId);
        }

        public List<Course> GetCourseByDepartment(int departmentId)
        {
            return admin.GetCourseByDepartment(departmentId);
        }

        public Teacher GetTeacherById(int teacherId)
        {
            return admin.GetTeacherById(teacherId);
        }

        public Course GetCourseById(int courseId)
        {
            return admin.GetCourseById(courseId);
        }

        public string AssignCourseToTeacher(AssignCourse assignCourse)
        {
            var rowAffected = admin.AssignCourseToTeacher(assignCourse);
            if (rowAffected > 0)
            {
                return "success";
            }
            else
            {
                return "failed";
            }
        }

        public List<AssignCourse> GetCoursesInformation(int departmentId)
        {
            List<AssignCourse> assignCourses = admin.GetCoursesInformation(departmentId);

            foreach (AssignCourse assignCourse in assignCourses)
            {
                Teacher teacher = new Teacher();
                teacher.TeacherName = admin.GetTeacherByCourseId(assignCourse.Course.CourseId);
                assignCourse.Teacher = teacher;
            }

            return assignCourses;
        }

        public List<Room> GetAllRooms()
        {
            return admin.GetAllRooms();
        }

        public List<Day> GetAllDays()
        {
            return admin.GetAllDays();
        }

        public bool IsCourseTaken(int courseId)
        {
            return admin.IsCourseTaken(courseId);
        }

        public string UpdateTeacherTakenCredit(double extraCredit, int teacherId)
        {
            if (admin.UpdateTeacherTakenCredit(extraCredit, teacherId) >= 0)
            {
                return "success";
            }
            return "failed";
        }

        public int GetTotalStudents()
        {
            return admin.GetTotalStudents();
        }

        public int GetTotalTeachers()
        {
            return admin.GetTotalTeachers();
        }

        public int GetTotalCourses()
        {
            return admin.GetTotalCourses();
        }

        public int GetTotalDepartments()
        {
            return admin.GetTotalDepartments();
        }

        public string AllocateClassroom(AllocatedClassroom allocatedClassroom)
        {
            if (admin.AllocateClassroom(allocatedClassroom)>= 0)
            {
                return "Success";
            }

            return "Failed";
        }

        public List<Course> GetClassroomInformation(int departmentId)
        {
            List<Course> courses = admin.GetCourseInformation(departmentId);
            return courses;
        }

        public string UnassignCourses(int code)
        {
            if (admin.UnassignCoursesFromCourseAssign(code) >= 0 && admin.UnassignCoursesFromEnrollCourse(code) >= 0)
            {
                return "Success";
            }

            return "Failed";
        }
        //public bool IsRoomAvailable(int roomId, int dayId, string fromTime, string toTime)
        //{
        //    return admin.IsRoomAvailable(roomId,dayId,fromTime,toTime);
        //}
        //Sirajuls code ends here




        //Soikats code starts here
        public List<Semester> GetSemesters()
        {
            return admin.GetSemesters();
        }
        public List<Designation> GetDesignations()
        {
            return admin.GetDesignations();
        }
        public string SaveCourse(Course course)
        {
            if (admin.SaveCourse(course) >= 0)
            {
                return "Added successfully";
            }
            else
            {
                return "Somethings went wrong";
            }
        }
        public string SaveTeacher(Teacher teacher)
        {

            if (admin.SaveTeacher(teacher) >= 0)
            {

                return "Added successfully";
            }
            else
            {
                return "Somethings went wrong";
            }
        }

        public bool IsCourseNameTaken(string courseName)
        {
            return admin.IsCourseNameTaken(courseName);
        }

        public bool IsCourseCodeTaken(string courseCode)
        {
            return admin.IsCourseCodeTaken(courseCode);
        }

        public bool IsTeacherEmailTaken(string teacherEmail)
        {
            return admin.IsTeacherEmailTaken(teacherEmail);
        }
        //Soikats code ends here



        public string UnallocateClassroom(int code)
        {
            if (admin.UnallocateClassroom(code) >= 0)
            {
                return "Success";
            }

            return "Failed";
        }
    }
}