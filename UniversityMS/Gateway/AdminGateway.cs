using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using UniversityMS.Models;

namespace UniversityMS.Gateway
{
    public class AdminGateway
    {
        //Sirajul code starts here
        public List<Department> GetAllDepartment()
        {
            string Query = "SELECT * FROM Department";
            Gateway gateway = new Gateway(Query);
            SqlDataReader reader = gateway.SqlCommand.ExecuteReader();
            List<Department> departments = new List<Department>();
            while (reader.Read())
            {
                Department department = new Department();
                department.DepartmentId = (int)reader["DepartmentId"];
                department.DepartmentCode = reader["DepartmentCode"].ToString();

                departments.Add(department);
            }

            reader.Close();
            gateway.Connection.Close();
            return departments;
        }

        public List<Teacher> GetTeacherByDepartment(int departmentId)
        {
            string query = "SELECT * FROM Teacher WHERE DepartmentId = @departmentId";
            Gateway gateway = new Gateway(query);
            gateway.SqlCommand.Parameters.Clear();
            gateway.SqlCommand.Parameters.AddWithValue("@departmentId", departmentId);
            SqlDataReader reader = gateway.SqlCommand.ExecuteReader();
            List<Teacher> teachers = new List<Teacher>();
            while (reader.Read())
            {
                Teacher teacher = new Teacher();
                teacher.TeacherId = (int)reader["TeacherId"];
                teacher.TeacherName = reader["TeacherName"].ToString();

                teachers.Add(teacher);
            }
            reader.Close();
            gateway.Connection.Close();
            return teachers;
        }

        public List<Course> GetCourseByDepartment(int departmentId)
        {
            List<Course> courses = new List<Course>();
            string query = "SELECT * FROM Course WHERE DepartmentId = @departmentId";
            Gateway gateway = new Gateway(query);
            gateway.SqlCommand.Parameters.Clear();
            gateway.SqlCommand.Parameters.AddWithValue("@departmentId", departmentId);
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

        public Teacher GetTeacherById(int teacherId)
        {
            string query = "SELECT * FROM Teacher WHERE TeacherId = @teacherId";
            Gateway gateway = new Gateway(query);
            gateway.SqlCommand.Parameters.Clear();
            gateway.SqlCommand.Parameters.AddWithValue("@teacherId", teacherId);
            SqlDataReader reader = gateway.SqlCommand.ExecuteReader();
            Teacher teacher = new Teacher();
            if (reader.HasRows)
            {
                reader.Read();
                teacher.TeacherId = (int)reader["TeacherId"];
                teacher.TeacherName = reader["TeacherName"].ToString();
                teacher.TakenCredit = (double)reader["TakenCredit"];
                teacher.RemainingCredit = GetTakenCourseCredit(teacherId, reader);
            }

            reader.Close();
            gateway.Connection.Close();
            return teacher;
        }

        public Course GetCourseById(int courseId)
        {
            string query = "SELECT * FROM Course WHERE CourseId = @courseId";
            Gateway gateway = new Gateway(query);
            gateway.SqlCommand.Parameters.Clear();
            gateway.SqlCommand.Parameters.AddWithValue("@courseId", courseId);
            SqlDataReader reader = gateway.SqlCommand.ExecuteReader();
            Course course = new Course();
            if (reader.HasRows)
            {
                reader.Read();
                course.CourseId = (int)reader["CourseId"];
                course.CourseCode = reader["CourseCode"].ToString();
                course.CourseName = reader["CourseName"].ToString();
                course.Credit = (double)reader["Credit"];
            }

            reader.Close();
            gateway.Connection.Close();
            return course;
        }


        public double GetTakenCourseCredit(int teacherId, SqlDataReader r)
        {
            r.Close();
            double takenCourseCredit = 0.0;
            string query = "SELECT TakenCourseCredit FROM CourseAssign WHERE TeacherId = @teacherId";
            Gateway gateway = new Gateway(query);
            gateway.SqlCommand.Parameters.Clear();
            gateway.SqlCommand.Parameters.AddWithValue("@teacherId", teacherId);
            SqlDataReader reader = gateway.SqlCommand.ExecuteReader();
            while (reader.Read())
            {
                takenCourseCredit += (double)reader[0];
            }

            reader.Close();
            gateway.Connection.Close();
            return takenCourseCredit;
        }

        public int AssignCourseToTeacher(AssignCourse assignCourse)
        {
            string query =
                "INSERT INTO CourseAssign(TeacherId, CourseId, TakenCourseCredit, Available)" +
                " VALUES(@teacherId, @courseId, @TakenCourseCredit, @available)";
            Gateway gateway = new Gateway(query);
            gateway.SqlCommand.Parameters.Clear();
            gateway.SqlCommand.Parameters.AddWithValue("@teacherId", assignCourse.TeacherId);
            gateway.SqlCommand.Parameters.AddWithValue("@courseId", assignCourse.CourseId);
            gateway.SqlCommand.Parameters.AddWithValue("@TakenCourseCredit", assignCourse.TakenCourseCredit);
            gateway.SqlCommand.Parameters.AddWithValue("@available", 1);

            int rowAffected = gateway.SqlCommand.ExecuteNonQuery();
            gateway.Connection.Close();
            return rowAffected;

        }

        public List<AssignCourse> GetCoursesInformation(int departmentId)
        {
            List<AssignCourse> assignCourses = new List<AssignCourse>();
            string query = "Select C.CourseId,C.CourseCode, C.CourseName,S.SemesterName From Course C " +
                    "Inner Join Semester S On S.SemesterId = C.SemesterId " +
                    "Where C.DepartmentId = @departmentId";
            Gateway gateway = new Gateway(query);
            gateway.SqlCommand.Parameters.Clear();
            gateway.SqlCommand.Parameters.AddWithValue("@departmentId", departmentId);
            SqlDataReader reader = gateway.SqlCommand.ExecuteReader();

            while (reader.Read())
            {
                AssignCourse assignCourse = new AssignCourse();
                Course course = new Course();
                course.CourseId = (int)reader["CourseId"];
                course.CourseCode = reader["CourseCode"].ToString();
                course.CourseName = reader["CourseName"].ToString();
                assignCourse.Course = course;
                Semester semester = new Semester();
                semester.SemesterName = reader["SemesterName"].ToString();
                assignCourse.Semester = semester;

                assignCourses.Add(assignCourse);
            }
            reader.Close();
            gateway.Connection.Close();

            return assignCourses;
        }

        public string GetTeacherByCourseId(int courseId)
        {
            string query = "select T.TeacherName from CourseAssign Ca " +
                           "Inner Join Teacher T on T.TeacherId = Ca.TeacherId " +
                           "Where Ca.CourseId = @courseId";
            Gateway gateway = new Gateway(query);
            gateway.SqlCommand.Parameters.Clear();
            gateway.SqlCommand.Parameters.AddWithValue("@courseId", courseId);
            SqlDataReader reader1 = gateway.SqlCommand.ExecuteReader();

            if (reader1.HasRows)
            {
                reader1.Read();
                string teacherName = reader1[0].ToString();
                reader1.Close();
                gateway.Connection.Close();
                return teacherName;
            }
            else
            {
                reader1.Close();
                gateway.Connection.Close();
                return "Not Assigned Yet";
            }
        }

        public List<Room> GetAllRooms()
        {
            string Query = "SELECT * FROM Room";
            Gateway gateway = new Gateway(Query);
            SqlDataReader reader = gateway.SqlCommand.ExecuteReader();
            List<Room> rooms = new List<Room>();
            while (reader.Read())
            {
                Room room = new Room();
                room.RoomId = (int)reader["RoomId"];
                room.RoomNo = reader["RoomNo"].ToString();

                rooms.Add(room);
            }

            reader.Close();
            gateway.Connection.Close();
            return rooms;
        }

        public List<Day> GetAllDays()
        {
            string Query = "SELECT * FROM Day ORDER BY DayId";
            Gateway gateway = new Gateway(Query);
            SqlDataReader reader = gateway.SqlCommand.ExecuteReader();
            List<Day> days = new List<Day>();
            while (reader.Read())
            {
                Day day = new Day();
                day.DayId = (int)reader["DayId"];
                day.DayName = reader["DayName"].ToString();

                days.Add(day);
            }

            reader.Close();
            gateway.Connection.Close();
            return days;
        }

        public bool IsCourseTaken(int courseId)
        {
            string query = "SELECT TOP 1 TeacherId FROM CourseAssign WHERE CourseId= @courseId and Available = 1";

            Gateway gateway = new Gateway(query);
            gateway.SqlCommand.Parameters.Clear();

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

        public int UpdateTeacherTakenCredit(double extraCredit, int teacherId)
        {
            string updateQuery = "UPDATE Teacher SET TakenCredit = @extraCredit WHERE TeacherId = @teacherId";
            Gateway gateway = new Gateway(updateQuery);
            gateway.SqlCommand.Parameters.Clear();
            gateway.SqlCommand.Parameters.AddWithValue("@extraCredit", extraCredit);
            gateway.SqlCommand.Parameters.AddWithValue("@teacherId", teacherId);

            int rowAffected = gateway.SqlCommand.ExecuteNonQuery();
            gateway.Connection.Close();
            return rowAffected;

        }

        


        public int GetTotalStudents()
        {
            int count = 0;
            string query = "Select Count(StudentId) Student from Student";
            Gateway gateway = new Gateway(query);
            SqlDataReader reader = gateway.SqlCommand.ExecuteReader();
            if (reader.HasRows)
            {
                reader.Read();
                count = (int)reader["Student"];
            }

            reader.Close();
            gateway.Connection.Close();
            return count;
        }

        public int GetTotalTeachers()
        {
            int count = 0;
            string query = "Select Count(TeacherId) Teacher from Teacher";
            Gateway gateway = new Gateway(query);
            SqlDataReader reader = gateway.SqlCommand.ExecuteReader();
            if (reader.HasRows)
            {
                reader.Read();
                count = (int)reader["Teacher"];
            }

            reader.Close();
            gateway.Connection.Close();
            return count;
        }

        public int GetTotalCourses()
        {
            int count = 0;
            string query = "Select Count(CourseId) Course from Course";
            Gateway gateway = new Gateway(query);
            SqlDataReader reader = gateway.SqlCommand.ExecuteReader();
            if (reader.HasRows)
            {
                reader.Read();
                count = (int)reader["Course"];
            }

            reader.Close();
            gateway.Connection.Close();
            return count;
        }

        public int GetTotalDepartments()
        {
            int count = 0;
            string query = "Select Count(DepartmentId) Department from Department";
            Gateway gateway = new Gateway(query);
            SqlDataReader reader = gateway.SqlCommand.ExecuteReader();
            if (reader.HasRows)
            {
                reader.Read();
                count = (int)reader["Department"];
            }

            reader.Close();
            gateway.Connection.Close();
            return count;
        }

        public int AllocateClassroom(AllocatedClassroom allocatedClassroom)
        {
            string query = "INSERT INTO AllocateClassroom (CourseId, RoomId, DayId,FromTime,ToTime)" +
                           "VALUES(@courseId, @roomId, @dayId, @fromTime, @toTime)";
            Gateway gateway = new Gateway(query);

            gateway.SqlCommand.Parameters.Clear();
            gateway.SqlCommand.Parameters.AddWithValue("@courseId", allocatedClassroom.CourseId);
            gateway.SqlCommand.Parameters.AddWithValue("@roomId", allocatedClassroom.RoomId);
            gateway.SqlCommand.Parameters.AddWithValue("@dayId", allocatedClassroom.DayId);
            gateway.SqlCommand.Parameters.AddWithValue("@fromTime", allocatedClassroom.FromTime);
            gateway.SqlCommand.Parameters.AddWithValue("@toTime", allocatedClassroom.ToTime);

            int rowAffected = gateway.SqlCommand.ExecuteNonQuery();
            gateway.Connection.Close();
            return rowAffected;
        }

        public List<Course> GetCourseInformation(int departmentId)
        {
            List<Course> courses = new List<Course>();
            string query = "Select C.CourseId,C.CourseCode, C.CourseName From Course C " +
                           "Where C.DepartmentId = @departmentId";
            Gateway gateway = new Gateway(query);
            gateway.SqlCommand.Parameters.Clear();
            gateway.SqlCommand.Parameters.AddWithValue("@departmentId", departmentId);
            SqlDataReader reader = gateway.SqlCommand.ExecuteReader();

            while (reader.Read())
            {
                Course course = new Course();
                course.CourseId = (int)reader["CourseId"];
                course.CourseCode = reader["CourseCode"].ToString();
                course.CourseName = reader["CourseName"].ToString();
                if (GetScheduleInfo(course.CourseId) == "")
                {
                    course.Schedule = "Not Schedule Yet";
                }
                else
                {
                    course.Schedule = GetScheduleInfo(course.CourseId);
                }
                
                courses.Add(course);
            }
            reader.Close();
            gateway.Connection.Close();

            return courses;
        }

        public string GetScheduleInfo(int courseId)
        {
            //List<AllocatedClassroom> allocatedClassrooms = new List<AllocatedClassroom>();
            string schedule = String.Empty;
            string query = "SELECT Ac.CourseId,R.RoomNo,Ac.FromTime,Ac.ToTime,D.DayName FROM AllocateClassroom Ac " +
                           "INNER JOIN Room R ON R.RoomId = Ac.RoomId " +
                           "INNER JOIN Day D ON D.DayId = Ac.DayId " +
                           "WHERE Ac.CourseId = @courseId";
            Gateway gateway = new Gateway(query);
            gateway.SqlCommand.Parameters.Clear();
            gateway.SqlCommand.Parameters.AddWithValue("@courseId", courseId);
            SqlDataReader reader = gateway.SqlCommand.ExecuteReader();

            while (reader.Read())
            {

                schedule += "R. No:" + reader["RoomNo"] + "," + reader["DayName"] + "," + reader["FromTime"] + "-" +
                            reader["ToTime"] + ";</br>";
                //AllocatedClassroom allocatedClassroom = new AllocatedClassroom(); 
                //Room room = new Room();
                //room.RoomNo = reader["RoomNo"].ToString();
                //allocatedClassroom.Room = room;
                //allocatedClassroom.FromTime = reader["FromTime"].ToString();
                //allocatedClassroom.ToTime = reader["ToTime"].ToString();
                //Day day = new Day();
                //day.DayName = reader["DayName"].ToString();
                //allocatedClassroom.Day = day;

                //allocatedClassrooms.Add(allocatedClassroom);
            }
            reader.Close();
            gateway.Connection.Close();
            return schedule;
            //return allocatedClassrooms;

        }

        public int UnassignCoursesFromCourseAssign(int code)
        {
            string updateQuery = "UPDATE CourseAssign SET Available = @code";
            Gateway gateway = new Gateway(updateQuery);
            gateway.SqlCommand.Parameters.Clear();
            gateway.SqlCommand.Parameters.AddWithValue("@code", code);

            int rowAffected = gateway.SqlCommand.ExecuteNonQuery();
            gateway.Connection.Close();
            return rowAffected;
        }
        public int UnassignCoursesFromEnrollCourse(int code)
        {
            string updateQuery = "UPDATE EnrollStudent SET Available = @code";
            Gateway gateway = new Gateway(updateQuery);
            gateway.SqlCommand.Parameters.Clear();
            gateway.SqlCommand.Parameters.AddWithValue("@code", code);

            int rowAffected = gateway.SqlCommand.ExecuteNonQuery();
            gateway.Connection.Close();
            return rowAffected;
        }
        //public bool IsRoomAvailable(int roomId, int dayId, string fromTime, string toTime)
        //{
        //    string query = "";
        //}
        //Sirajuls code ends here



        //Soikats code starts here
        public List<Semester> GetSemesters()
        {
            List<Semester> semesters = new List<Semester>();

            string Query = "SELECT * FROM Semester";
            Gateway gateway = new Gateway(Query);
            SqlDataReader reader = gateway.SqlCommand.ExecuteReader();
            while (reader.Read())
            {
                Semester semester = new Semester();
                semester.SemesterId = (int)reader["SemesterId"];
                semester.SemesterName = reader["SemesterName"].ToString();

                semesters.Add(semester);
            }

            reader.Close();
            gateway.Connection.Close();
            return semesters;

        }
        public List<Designation> GetDesignations()
        {
            List<Designation> designations = new List<Designation>();

            string Query = "SELECT * FROM Designation";
            Gateway gateway = new Gateway(Query);
            SqlDataReader reader = gateway.SqlCommand.ExecuteReader();
            while (reader.Read())
            {

                Designation designation = new Designation();
                designation.DesignationId = (int)reader["DesignationId"];
                designation.DesignationName = reader["DesignationName"].ToString();

                designations.Add(designation);
            }

            reader.Close();
            gateway.Connection.Close();
            return designations;

        }
        public int SaveCourse(Course course)
        {
            string query = "INSERT INTO Course (CourseName,CourseCode,Credit,Description,DepartmentId,SemesterId)" +
                           " VALUES (@courseName, @courseCode,@credit,@description,@departmentId,@semesterId)";
            Gateway gateway = new Gateway(query);
            gateway.SqlCommand.Parameters.Clear();
            gateway.SqlCommand.Parameters.AddWithValue("@courseName", course.CourseName);
            gateway.SqlCommand.Parameters.AddWithValue("@courseCode", course.CourseCode);
            gateway.SqlCommand.Parameters.AddWithValue("@credit", course.Credit);
            gateway.SqlCommand.Parameters.AddWithValue("@description", course.Description);
            gateway.SqlCommand.Parameters.AddWithValue("@departmentId", course.DepartmentId);
            gateway.SqlCommand.Parameters.AddWithValue("@semesterId", course.SemesterId);

            int rowAffected = gateway.SqlCommand.ExecuteNonQuery(); 
            gateway.Connection.Close();
            return rowAffected;
        }
        public int SaveTeacher(Teacher teacher)
        {
            string query = "INSERT INTO Teacher (TeacherName,TeacherAddress,TeacherEmail,TeacherContactNo,DesignationId,DepartmentId,TakenCredit)" +
                           " VALUES (@teacherName, @teacherAddress,@teacherEmail,@teacherContactNo,@designationId,@departmentId,@takenCredit)";
            Gateway gateway = new Gateway(query);
            gateway.SqlCommand.Parameters.Clear();
            gateway.SqlCommand.Parameters.AddWithValue("@teacherName", teacher.TeacherName);
            gateway.SqlCommand.Parameters.AddWithValue("@teacherAddress", teacher.TeacherAddress);
            gateway.SqlCommand.Parameters.AddWithValue("@teacherEmail", teacher.TeacherEmail);
            gateway.SqlCommand.Parameters.AddWithValue("@teacherContactNo", teacher.TeacherContactNo);
            gateway.SqlCommand.Parameters.AddWithValue("@designationId", teacher.DesignationId);
            gateway.SqlCommand.Parameters.AddWithValue("@departmentId", teacher.DepartmentId);
            gateway.SqlCommand.Parameters.AddWithValue("@takenCredit", teacher.TakenCredit);

            int rowAffected = gateway.SqlCommand.ExecuteNonQuery(); 
            gateway.Connection.Close();
            return rowAffected;
        }

        public bool IsCourseNameTaken(string courseName)
        {
            string query = "SELECT TOP 1 CourseId FROM Course WHERE CourseName= @courseName";

            Gateway gateway = new Gateway(query);
            gateway.SqlCommand.Parameters.Clear();

            gateway.SqlCommand.Parameters.AddWithValue("@courseName", courseName);

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

        public bool IsCourseCodeTaken(string courseCode)
        {
            string query = "SELECT TOP 1 CourseId FROM Course WHERE CourseCode= @courseCode";

            Gateway gateway = new Gateway(query);
            gateway.SqlCommand.Parameters.Clear();

            gateway.SqlCommand.Parameters.AddWithValue("@courseCode", courseCode);

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

        public bool IsTeacherEmailTaken(string teacherEmail)
        {
            string query = "SELECT TOP 1 TeacherId FROM Teacher WHERE TeacherEmail= @teacherEmail";

            Gateway gateway = new Gateway(query);
            gateway.SqlCommand.Parameters.Clear();

            gateway.SqlCommand.Parameters.AddWithValue("@teacherEmail", teacherEmail);

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
        //Soikats code ends here





        public int UnallocateClassroom(int code)
        {
            string updateQuery = "UPDATE AllocateClassroom SET Available = @code";
            Gateway gateway = new Gateway(updateQuery);
            gateway.SqlCommand.Parameters.Clear();
            gateway.SqlCommand.Parameters.AddWithValue("@code", code);

            int rowAffected = gateway.SqlCommand.ExecuteNonQuery();
            gateway.Connection.Close();
            return rowAffected;
        }
    }
}