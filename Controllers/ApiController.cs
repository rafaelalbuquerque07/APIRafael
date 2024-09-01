using APIRafael.Models;
using APIRafael.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        public async Task<ActionResult<List<Student>>> GetStudents()
        {
            var students = await _studentRepository.GetStudentsAsync();
            return Ok(students);
        }

        [HttpPost(Name = "AddStudent")]
        public async Task<ActionResult<string>> AddStudent(Student student)
        {
            var addedStudent = await _studentRepository.AddStudentAsync(student);
            return Ok($"Estudante {addedStudent.Name} adicionado com ID {addedStudent.ID}!");
        }

        [HttpPut(Name = "UpdateStudent")]
        public async Task<ActionResult<string>> UpdateStudent(Student student)
        {
            var updatedStudent = await _studentRepository.UpdateStudentAsync(student.ID, student);
            return Ok($"Estudante {updatedStudent.Name} atualizado!");
        }

        [HttpGet("{id}", Name = "GetStudentById")]
        public async Task<ActionResult<Student>> GetStudentById(int id)
        {
            var student = await _studentRepository.GetStudentByIdAsync(id);
            if (student == null)
            {
                return NotFound(); // Retorna 404 se o estudante não for encontrado
            }
            return Ok(student);
        }

        [HttpDelete(Name = "DeleteStudent")]
        public async Task<ActionResult<string>> DeleteStudent(int id)
        {
            var success = await _studentRepository.DeleteStudentAsync(id);
            if (!success)
            {
                return NotFound($"Estudante do id {id} não encontrado.");
            }
            return Ok($"Estudante do id {id} deletado!");
        }
    }
}
