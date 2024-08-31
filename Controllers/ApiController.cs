using APIRafael.Models;
using APIRafael.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace APIRafael.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ApiController : ControllerBase
    {
        private readonly StudentRepository _studentRepository;

        public ApiController(StudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        [HttpGet(Name = "GetStudents")]
        public ActionResult<List<Student>> GetStudents()
        {
            var students = _studentRepository.GetStudents();
            return Ok(students);
        }

        [HttpPost(Name = "AddStudents")]
        public ActionResult<string> AddStudent(Student student)
        {
            _studentRepository.AddStudent(student);
            return Ok($"Estudante {student.Name} adicionado!");
        }

        [HttpPut(Name = "UpdateStudent")]
        public ActionResult<string> UpdateStudent(Student student)
        {
            _studentRepository.UpdateStudent(student);
            return Ok($"Estudante {student.Name} ATUALIZADO!");
        }

        [HttpGet("{id}", Name = "GetStudentById")]
        public ActionResult<Student> GetStudentById(int id)
        {
            var student = _studentRepository.GetStudentById(id);
            if (student == null)
            {
                return NotFound(); // Retorna 404 se o estudante não for encontrado
            }
            return Ok(student);
        }

        [HttpDelete(Name = "DeleteStudent")]
        public ActionResult<string> DeleteStudent(int id)
        {
            _studentRepository.DeleteStudent(id);
            return Ok($"Estudante do id {id} deletado!");
        }
    }
}
