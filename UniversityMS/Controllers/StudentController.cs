using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using iTextSharp.text.html.simpleparser;
using Rotativa;
using Rotativa.MVC;
using UniversityMS.Manager;
using UniversityMS.Models;
using Font = iTextSharp.text.Font;

namespace UniversityMS.Controllers
{
    public class StudentController : Controller
    {
        StudentManager studentManager = new StudentManager();
        AdminManager admin = new AdminManager();
        //Sirajuls code start here
        public ActionResult ViewStudentResult()
        {
            ViewData["students"] = new SelectList(studentManager.GetStudents(), "StudentId", "RegistrationCode");
            return View();
        }

        [HttpPost]
        [ActionName("ViewStudentResult")]
        public ActionResult CreatePdf(Student student)
        {
            Document pdfDoc = new Document(PageSize.A4, 25, 25, 25, 15);
            PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
            pdfDoc.Open();

            //Top Heading
            Chunk chunk = new Chunk("Your Result Report has been Generated", FontFactory.GetFont("Arial", 20, Font.BOLDITALIC, BaseColor.BLACK));
            pdfDoc.Add(chunk);

            //Horizontal Line
            Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_MIDDLE, 1)));
            pdfDoc.Add(line);

            //Table
            PdfPTable table = new PdfPTable(1);
            table.WidthPercentage = 100;
            //0=Left, 1=Centre, 2=Right
            table.HorizontalAlignment = 0;
            table.SpacingBefore = 20f;
            table.SpacingAfter = 30f;

            Student newStudent = studentManager.GetStudent(student.StudentId);
            //Cell no 2
            chunk = new Chunk("Reg no: "+newStudent.RegistrationCode+"\nName: "+newStudent.StudentName +" \nEmail: "+newStudent.StudentEmail+" \nDepartment: "+newStudent.Department.DepartmentName+"", FontFactory.GetFont("Arial", 15, Font.NORMAL, BaseColor.BLACK));
            PdfPCell cell = new PdfPCell();
            cell.Border = 0;
            cell.AddElement(chunk);
            table.AddCell(cell);

            //Add table to document
            pdfDoc.Add(table);

            //Horizontal Line
            line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
            pdfDoc.Add(line);

            //Table
            table = new PdfPTable(3);
            table.WidthPercentage = 100;
            table.HorizontalAlignment = 0;
            table.SpacingBefore = 20f;
            table.SpacingAfter = 30f;

            //Cell



            List<Course> courses = studentManager.GetCourses(student.StudentId);

            cell = new PdfPCell();
            chunk = new Chunk("Course Code");
            chunk.Font.Color = BaseColor.WHITE;
            cell.AddElement(chunk);
            cell.BackgroundColor = new BaseColor(21,140,186);
            table.AddCell(cell);

            cell = new PdfPCell();
            chunk = new Chunk("Name");
            chunk.Font.Color = BaseColor.WHITE;
            cell.AddElement(chunk);
            cell.BackgroundColor = new BaseColor(21, 140, 186);
            table.AddCell(cell);

            cell = new PdfPCell();
            chunk = new Chunk("Grade");
            chunk.Font.Color = BaseColor.WHITE;
            cell.AddElement(chunk);
            cell.BackgroundColor = new BaseColor(21, 140, 186);
            table.AddCell(cell);

            foreach (Course aCourse in courses)
            {
                table.AddCell(aCourse.CourseCode);
                table.AddCell(aCourse.CourseName);
                table.AddCell(aCourse.Grade.GradeCode);
            }

            pdfDoc.Add(table);
            pdfWriter.CloseStream = false;
            pdfDoc.Close();
            Response.Buffer = true;
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=Credit-Card-Report.pdf");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Write(pdfDoc);
            Response.End();
            ViewData["students"] = new SelectList(studentManager.GetStudents(), "StudentId", "RegistrationCode");
            return View();
        }
        [HttpPost]
        public ActionResult GetStudentInfo(int studentId)
        {
            Student student = studentManager.GetStudent(studentId);
            TempData["student"] = student;
            return Json(student, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetCourses(int studentId)
        {
            List<Course> courses = studentManager.GetCourses(studentId);
            TempData["courses"] = courses;
            return Json(courses, JsonRequestBehavior.AllowGet);
        }
        //Sirajuls code start here

        //Soikats code starts here
        [HttpGet]
        public ActionResult EnrollStudent()
        {
            ViewData["students"] = new SelectList(studentManager.GetStudents(), "StudentId", "RegistrationCode");
            return View();
        }
        [HttpPost]
        public ActionResult EnrollStudent(EnrollStudent enrollStudent)
        {
            string message = studentManager.EnrollStudent(enrollStudent);
            return Json(message, JsonRequestBehavior.AllowGet);
        }
        //Soikats code ends here

        //Ujjwal Code starts here
        [HttpGet]
        public ActionResult RegisterStudent()
        {
            ViewData["departments"] = new SelectList(admin.GetAllDepartment(), "DepartmentId", "DepartmentCode");
            return View();
        }

        [HttpPost]
        public ActionResult RegisterStudent(Student student)
        {
            string message = studentManager.RegisterStudent(student);
            return Json(message,JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult SaveStudentResult()
        {
            ViewData["students"] = new SelectList(studentManager.GetStudents(), "StudentId", "RegistrationCode");
            ViewData["grades"] = new SelectList(studentManager.GetGradeList(), "GradeId", "GradeCode");
            return View();
        }
        
        [HttpPost]
        public ActionResult SaveStudentResult(StudentResult studentResult)
        {
            string message = studentManager.SaveStudentResult(studentResult);
            return Json(message,JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult UpdateStudentResult(StudentResult studentResult)
        {
            string message = studentManager.UpdateStudentResult(studentResult);
            return Json(message, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCoursesByEnrollId(int studentId)
        {
            SelectList coursesList = new SelectList(studentManager.GetCoursesByEnrollId(studentId), "CourseId", "CourseCode");
            return Json(coursesList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCoursesByDepartmentlId(int studentId)
        {
            SelectList coursesList = new SelectList(studentManager.GetCoursesByDepartmentlId(studentId), "CourseId", "CourseCode");
            return Json(coursesList, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult GetStudentById(int studentId)
        {
            Student student = studentManager.GetStudentById(studentId);
            return Json(student, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult IsResultAlreadyExists(int courseId, int studentId)
        {
            return Json(studentManager.IsResultAlreadyExists(courseId, studentId), JsonRequestBehavior.AllowGet);
        }

        public JsonResult IsCourseEnrolled(int courseId,int studentRegEId)
        {
            return Json(!studentManager.IsCourseEnrolled(courseId,studentRegEId), JsonRequestBehavior.AllowGet);
        }

        public JsonResult IsStudentEmailExists(string studentEmail)
        {
            return Json(!studentManager.IsStudentEmailExists(studentEmail), JsonRequestBehavior.AllowGet);
        }
        //Ujjwal Code ends here
    }
}