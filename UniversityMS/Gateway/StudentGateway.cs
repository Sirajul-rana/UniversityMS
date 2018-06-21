using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using UniversityMS.Models;

namespace UniversityMS.Gateway
{
    public class StudentGateway
    {
        //Sirajuls code starts here
        public List<Student> GetStudents()
        {
            List<Student> students = new List<Student>();
            string query = "SELECT * FROM Student";
            Gateway gateway = new Gateway(query);
            gateway.SqlCommand.Parameters.Clear();
            SqlDataReader reader = gateway.SqlCommand.ExecuteReader();
            while (reader.Read())
            {
                Student student = new Student();
                student.StudentId = (int)reader["StudentId"];
                student.RegistrationCode = reader["RegistrationCode"].ToString();

                students.Add(student);
            }
            return students;
        }

        public Student GetStudent(int studentId)
        {
            string query = "select S.StudentId,S.StudentName,S.StudentEmail,S.RegistrationCode,S.DepartmentId from StudentResult Sr " +
                           "Inner Join Student S On S.StudentId = Sr.StudentRegId " +
                           "where Sr.StudentRegId = @studentId " +
                           "Group by S.StudentId,S.StudentName,S.StudentEmail,S.DepartmentId,S.RegistrationCode";
            Gateway gateway = new Gateway(query);
            gateway.SqlCommand.Parameters.Clear();
            gateway.SqlCommand.Parameters.AddWithValue("@studentId", studentId);
            SqlDataReader reader = gateway.SqlCommand.ExecuteReader();
            Student student = new Student();
            if (reader.HasRows)
            {
                reader.Read();

                student.StudentId = (int)reader["StudentId"];
                student.StudentName = reader["StudentName"].ToString();
                student.RegistrationCode = reader["RegistrationCode"].ToString();
                student.StudentEmail = reader["StudentEmail"].ToString();
                student.DepartmentId = (int)reader["DepartmentId"];
                
            }
            reader.Close();
            gateway.Connection.Close();
            return student;
        }

        public List<Course> GetCourses(int studentId)
        {

            string query = "select C.CourseCode,C.CourseName,G.GradeCode from StudentResult Sr " +
                           "Inner Join Course C On C.CourseId = Sr.CourseId " +
                           "Inner Join Grade G On G.GradeId = Sr.GradeId " +
                           "where Sr.StudentRegId = @studentId";
            Gateway gateway = new Gateway(query);
            gateway.SqlCommand.Parameters.Clear();
            gateway.SqlCommand.Parameters.AddWithValue("@studentId", studentId);
            SqlDataReader reader = gateway.SqlCommand.ExecuteReader();
            List<Course> courses = new List<Course>();
            while (reader.Read())
            {
                Course course = new Course();
                course.CourseCode = reader["CourseCode"].ToString();
                course.CourseName = reader["CourseName"].ToString();
                Grade grade = new Grade();
                if (reader["GradeCode"].ToString() == "No Grade")
                {
                    grade.GradeCode = "Not Graded Yet";
                }
                else
                {
                    grade.GradeCode = reader["GradeCode"].ToString();
                }
                
                course.Grade = grade;

                courses.Add(course);
            }
            gateway.Connection.Close();
            return courses;
        }

        public string GetDepartmentByStudentId(int departmentId)
        {
            string query = "SELECT * FROM Department WHERE DepartmentId = @departmentId";
            Gateway gateway = new Gateway(query);
            gateway.SqlCommand.Parameters.Clear();
            gateway.SqlCommand.Parameters.AddWithValue("@departmentId", departmentId);
            SqlDataReader reader = gateway.SqlCommand.ExecuteReader();
            string departmentName = String.Empty;

            if (reader.HasRows)
            {
                reader.Read();
                departmentName = reader["DepartmentName"].ToString();
            }
            reader.Close();
            gateway.Connection.Close();
            return departmentName;
        }
        //Sirajuls code ends here




        //Ujjwal code starts here 
        public int RegisterStudent(Student student)
        {
            string query = "INSERT INTO Student (StudentName,StudentEmail,StudentContactNo,RegisterDate,StudentAddress,DepartmentId,RegistrationCode)" +
                           " VALUES (@studentName, @studentEmail,@studentContactNo,@registerDate,@studentAddress,@departmentId,@registrationCode)";
            Gateway gateway = new Gateway(query);
            gateway.SqlCommand.Parameters.Clear();
            gateway.SqlCommand.Parameters.AddWithValue("@studentName", student.StudentName);
            gateway.SqlCommand.Parameters.AddWithValue("@studentEmail", student.StudentEmail);
            gateway.SqlCommand.Parameters.AddWithValue("@studentContactNo", student.StudentContactNo);
            gateway.SqlCommand.Parameters.AddWithValue("@registerDate", student.RegisterDate);
            gateway.SqlCommand.Parameters.AddWithValue("@studentAddress", student.StudentAddress);
            gateway.SqlCommand.Parameters.AddWithValue("@departmentId", student.DepartmentId);
            gateway.SqlCommand.Parameters.AddWithValue("@registrationCode", GetDepartmentCode(student.DepartmentId) + "-" + DateTime.Parse(student.RegisterDate.ToString()).Year + "-" + (Convert.ToInt32(GetLastStudentId()) + 1));

            int rowAffected = gateway.SqlCommand.ExecuteNonQuery();
            return rowAffected;
        }

        public string GetLastStudentId()
        {
            string lastStudentId = String.Empty;
            string query = "SELECT IDENT_CURRENT('Student')";
            Gateway gateway = new Gateway(query);
            gateway.SqlCommand.Parameters.Clear();
            SqlDataReader reader = gateway.SqlCommand.ExecuteReader();

            if (reader.HasRows)
            {
                reader.Read();
                lastStudentId = reader[0].ToString();
            }

            return lastStudentId;
        }

        public string GetDepartmentCode(int departmentId)
        {
            string query = "SELECT * FROM Department WHERE DepartmentId = @departmentId";
            Gateway gateway = new Gateway(query);
            gateway.SqlCommand.Parameters.Clear();
            gateway.SqlCommand.Parameters.AddWithValue("@departmentId", departmentId);
            SqlDataReader reader = gateway.SqlCommand.ExecuteReader();
            string departmentCode = String.Empty;

            if (reader.HasRows)
            {
                reader.Read();
                departmentCode = reader["DepartmentCode"].ToString();
            }
            reader.Close();
            gateway.Connection.Close();
            return departmentCode;
        }
        
        public List<Course> GetCoursesByEnrollId(int studentId)
        {
            List<Course> courses = new List<Course>();
            string query = "select C.CourseId,C.CourseCode from EnrollStudent Es " +
                           "Inner join Course C on C.CourseId = Es.CourseId " +
                           "where Es.StudentId = @studentId";
            Gateway gateway = new Gateway(query);
            gateway.SqlCommand.Parameters.Clear();
            gateway.SqlCommand.Parameters.AddWithValue("@studentId", studentId);
            SqlDataReader reader = gateway.SqlCommand.ExecuteReader();

            while (reader.Read())
            {
                Course course = new Course();
                course.CourseId = (int)reader["CourseId"];
                course.CourseCode = reader["CourseCode"].ToString();
                courses.Add(course);
            }

            reader.Close();
            gateway.Connection.Close();
            return courses;
        }

        public List<Course> GetCoursesByDepartmentlId(int studentId)
        {
            List<Course> courses = new List<Course>();
            string query = "SELECT C.CourseId,C.CourseCode FROM Student S " +
                           "INNER JOIN Course C ON S.DepartmentId = C.DepartmentId " +
                           "WHERE S.StudentId = @studentId";
            Gateway gateway = new Gateway(query);
            gateway.SqlCommand.Parameters.Clear();
            gateway.SqlCommand.Parameters.AddWithValue("@studentId", studentId);
            SqlDataReader reader = gateway.SqlCommand.ExecuteReader();

            while (reader.Read())
            {
                Course course = new Course();
                course.CourseId = (int)reader["CourseId"];
                course.CourseCode = reader["CourseCode"].ToString();
                courses.Add(course);
            }

            reader.Close();
            gateway.Connection.Close();
            return courses;
        }
        public List<Grade> GetGradeList()
        {
            List<Grade> grades = new List<Grade>();
            string query = "SELECT * FROM Grade order by GradeId";
            Gateway gateway = new Gateway(query);
            gateway.SqlCommand.Parameters.Clear();
            SqlDataReader reader = gateway.SqlCommand.ExecuteReader();

            while (reader.Read())
            {
                Grade grade = new Grade();
                grade.GradeId = (int)reader["GradeId"];
                grade.GradeCode = reader["GradeCode"].ToString();
                grades.Add(grade);
            }

            reader.Close();
            gateway.Connection.Close();
            return grades;
        }
        public Student GetStudentById(int studentId)
        {
            string query = "select S.StudentId,S.StudentName,S.StudentEmail,S.DepartmentId from Student S " +
                           "where S.StudentId = @studentId";
            Gateway gateway = new Gateway(query);
            gateway.SqlCommand.Parameters.Clear();
            gateway.SqlCommand.Parameters.AddWithValue("@studentId", studentId);
            SqlDataReader reader = gateway.SqlCommand.ExecuteReader();
            Student student = new Student();
            if (reader.HasRows)
            {
                reader.Read();

                student.StudentId = (int)reader["StudentId"];
                student.StudentName = reader["StudentName"].ToString();
                student.StudentEmail = reader["StudentEmail"].ToString();
                student.DepartmentId = (int)reader["DepartmentId"];

            }
            reader.Close();
            gateway.Connection.Close();
            return student;
        }
        public int SaveStudentResult(StudentResult studentResult)
        {
            string query = "INSERT INTO StudentResult (StudentRegId, CourseId,GradeId)" +
                           " VALUES (@studentRegId, @courseId,@gradeId)";
            Gateway gateway = new Gateway(query);
            gateway.SqlCommand.Parameters.Clear();
            gateway.SqlCommand.Parameters.AddWithValue("@studentRegId", studentResult.StudentRegId);
            gateway.SqlCommand.Parameters.AddWithValue("@courseId", studentResult.CourseId);
            gateway.SqlCommand.Parameters.AddWithValue("@gradeId", studentResult.GradeId);

            int rowAffected = gateway.SqlCommand.ExecuteNonQuery();
            gateway.Connection.Close();
            return rowAffected;
        }
        public int UpdateStudentResult(StudentResult studentResult)
        {
            string query = "UPDATE StudentResult SET GradeId = @gradeId where StudentRegId=@studentId";

            Gateway gateway = new Gateway(query);
            gateway.SqlCommand.Parameters.Clear();
            gateway.SqlCommand.Parameters.AddWithValue("@studentId", studentResult.StudentRegId);
            gateway.SqlCommand.Parameters.AddWithValue("@gradeId", studentResult.CourseId);
            int rowAffected = gateway.SqlCommand.ExecuteNonQuery();
            gateway.Connection.Close();
            return rowAffected;
        }

        public bool IsResultAlreadyExists(int courseId, int studentId)
        {
            string query = "SELECT TOP 1 GradeId FROM StudentResult WHERE CourseId=@courseId AND StudentRegId=@studentId";
            Gateway gateway = new Gateway(query);
            gateway.SqlCommand.Parameters.Clear();
            gateway.SqlCommand.Parameters.AddWithValue("@courseId", courseId);
            gateway.SqlCommand.Parameters.AddWithValue("@studentId", studentId);

            SqlDataReader reader = gateway.SqlCommand.ExecuteReader();
            if (reader.HasRows)
            {
                reader.Close();
                gateway.Connection.Close();
                return true;
            }
            else
            {
                reader.Close();
                gateway.Connection.Close();
                return false;
            }
        }

        public int EnrollStudent(EnrollStudent enrollStudent)
        {
            string query = "INSERT INTO EnrollStudent (StudentRegEId, CourseId,EnrollTime,Available)" +
                           " VALUES (@studentId, @courseId,@enrollTime,@available)";
            Gateway gateway = new Gateway(query);
            gateway.SqlCommand.Parameters.Clear();
            gateway.SqlCommand.Parameters.AddWithValue("@studentId", enrollStudent.StudentRegEId);
            gateway.SqlCommand.Parameters.AddWithValue("@courseId", enrollStudent.CourseId);
            gateway.SqlCommand.Parameters.AddWithValue("@enrollTime", enrollStudent.EnrollTime);
            gateway.SqlCommand.Parameters.AddWithValue("@available", 1);

            int rowAffected = gateway.SqlCommand.ExecuteNonQuery();
            return rowAffected;
        }
        public bool IsCourseEnrolled(int courseId, int studentId)
        {
            string query = "SELECT TOP 1 CourseId FROM EnrollStudent WHERE StudentRegEId = @studentId and CourseId= @courseId and Available = 1";

            Gateway gateway = new Gateway(query);
            gateway.SqlCommand.Parameters.Clear();

            gateway.SqlCommand.Parameters.AddWithValue("@studentId", studentId);
            gateway.SqlCommand.Parameters.AddWithValue("@courseId", courseId);

            SqlDataReader reader = gateway.SqlCommand.ExecuteReader();

            if (reader.HasRows)
            {
                reader.Close();
                gateway.Connection.Close();
                return true;
            }
            else
            {
                reader.Close();
                gateway.Connection.Close();
                return false;
            }
        }
        public bool IsStudentEmailExists(string studentEmail)
        {
            string query = "SELECT TOP 1 StudentId FROM Student WHERE StudentEmail= @studentEmail";

            Gateway gateway = new Gateway(query);
            gateway.SqlCommand.Parameters.Clear();

            gateway.SqlCommand.Parameters.AddWithValue("@studentEmail", studentEmail);

            SqlDataReader reader = gateway.SqlCommand.ExecuteReader();

            if (reader.HasRows)
            {
                reader.Close();
                gateway.Connection.Close();
                return true;
            }
            else
            {
                reader.Close();
                gateway.Connection.Close();
                return false;
            }
        }
        //Ujjwal code ends here 

















        
    }
}