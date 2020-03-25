using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Cw3.DAL;
using Cw3.Models;
using Microsoft.AspNetCore.Mvc;

namespace Cw3.Controllers
{

    [ApiController]
    [Route("api/students")]
    public class StudentsController : ControllerBase
    {

        private readonly IDbService _dbService;


        public StudentsController(IDbService dbService)
        {
            _dbService = dbService;
        }

        [HttpGet]
        public IActionResult GetStudent(){

        List<Student> students = new List<Student>();
            using (var con = new SqlConnection("Data Source=db-mssql;Initial Catalog=s18803;Integrated Security=True"))
            using (var com = new SqlCommand())
            {
                com.Connection = con;
                com.CommandText =
                    "SELECT FirstName, LastName, BirthDate, Semester, Name FROM Student " +
                    "INNER JOIN Enrollment ON Student.IdEnrollment = Enrollment.IdEnrollment " +
                    "INNER JOIN Studies ON Studies.IdStudy = Enrollment.IdStudy";

                con.Open();
                var dr = com.ExecuteReader();
                while (dr.Read())
                {
                    var student = new Student();
                    student.FirstName = dr["FirstName"].ToString();
                    student.LastName = dr["LastName"].ToString();
                    student.BirthDate = dr["BirthDate"].ToString();
                    student.Semester = dr["Semester"].ToString();
                    student.NameOfStudies = dr["Name"].ToString();
                    students.Add(student);
                }

            }

            return Ok(students);
        }


        [HttpGet("{id}")]
        public IActionResult GetStudentEnrollments(string id)
        {
            List<Enrollment> enrollments = new List<Enrollment>();

            using (var con = new SqlConnection("Data Source=db-mssql;Initial Catalog=s18728;Integrated Security=True"))
            using (var com = new SqlCommand())
            {
                com.Connection = con;
                com.CommandText = "SELECT * FROM Enrollment INNER JOIN Student ON Enrollment.IdEnrollment = Student.IdEnrollment WHERE Student.IndexNumber = @id";
                com.Parameters.AddWithValue("id", id);

                con.Open();
                var dr = com.ExecuteReader();
                while (dr.Read())
                {
                    var enrollment = new Enrollment();
                    enrollment.IdEnrollment = dr["IdEnrollment"].ToString();
                    enrollment.Semester = dr["Semester"].ToString();
                    enrollment.IdStudy = dr["IdStudy"].ToString();
                    enrollment.StartDate = dr["StartDate"].ToString();
                    enrollments.Add(enrollment);
                }

                return Ok(enrollments);
            }

        }


        [HttpGet]
        public IActionResult GetStudent(string orderBy)
        {
            return Ok(_dbService.GetStudents());
        }


        [HttpPost]
        public IActionResult CreateStudent(Models.Student student)
        {
            student.IndexNumber = $"s{new Random().Next(1, 20000)}";
            return Ok(student);
        }


        [HttpPut("{id}")]
        public IActionResult Put(int id)
        {
            return Ok("Aktualizacja ukończona: " + id);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return Ok("Usuwanie ukończone: " + id);
        }


        [HttpGet("{id}")]
        public IActionResult GetStudent(int id)
        {
            if (id == 1)
            {
                return Ok("Kowalski");
            }
            else if (id == 2)
            {
                return Ok("Malewski");
            }

            return NotFound("Nie znaleziono studenta");
            }
        }


    //[HttpGet]
    //public string GetStudent(string orderBy)
    //{
    //    return $"Kowalski, Malewski, Andrzejewski sortowanie={orderBy}";
    //}
}