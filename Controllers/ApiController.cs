using APIRafael.Models;
using APIRafael.Services;
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
        private readonly StudentsService _studentsService;
        private readonly ILogger<ApiController> _logger;

        public ApiController(StudentsService studentsService, ILogger<ApiController> logger)
        {
            _studentsService = studentsService;
            _logger = logger;
        }

        [HttpGet(Name = "GetStudents")]
        public async Task<ActionResult<List<Student>>> GetStudents()
        {
            _logger.LogInformation("Obtendo a lista de estudantes.");
            var students = await _studentsService.GetAllStudentsAsync();
            return Ok(students);
        }

        [HttpPost(Name = "AddStudent")]
        public async Task<ActionResult<string>> AddStudent(Student student)
        {
            if (student == null)
            {
                _logger.LogWarning("O estudante recebido é nulo.");
                return BadRequest("O estudante não pode ser nulo.");
            }

            _logger.LogInformation("Adicionando um novo estudante: {Name}", student.Name);
            var addedStudent = await _studentsService.AddStudentAsync(student);
            if (addedStudent == null)
            {
                _logger.LogError("Falha ao adicionar estudante: {Name}.", student.Name);
                return StatusCode(500, "Erro ao adicionar o estudante.");
            }
            _logger.LogInformation("Estudante {Name} adicionado com ID {ID}.", addedStudent.Name, addedStudent.ID);
            return Ok($"Estudante {addedStudent.Name} adicionado com ID {addedStudent.ID}!");
        }

        [HttpPut(Name = "UpdateStudent")]
        public async Task<ActionResult<string>> UpdateStudent(Student student)
        {
            if (student == null)
            {
                _logger.LogWarning("O estudante recebido é nulo.");
                return BadRequest("O estudante não pode ser nulo.");
            }

            _logger.LogInformation("Atualizando estudante com ID {ID}.", student.ID);
            var updatedStudent = await _studentsService.UpdateStudentAsync(student.ID, student);
            if (updatedStudent == null)
            {
                _logger.LogWarning("Estudante com ID {ID} não encontrado.", student.ID);
                return NotFound($"Estudante com ID {student.ID} não encontrado.");
            }
            _logger.LogInformation("Estudante {Name} atualizado.", updatedStudent.Name);
            return Ok($"Estudante {updatedStudent.Name} atualizado!");
        }

        [HttpGet("{id}", Name = "GetStudentById")]
        public async Task<ActionResult<Student>> GetStudentById(int id)
        {
            _logger.LogInformation("Obtendo estudante com ID {ID}.", id);
            var student = await _studentsService.GetStudentByIdAsync(id);
            if (student == null)
            {
                _logger.LogWarning("Estudante com ID {ID} não encontrado.", id);
                return NotFound(); // Retorna 404 se o estudante não for encontrado
            }
            return Ok(student);
        }

        [HttpDelete(Name = "DeleteStudent")]
        public async Task<ActionResult<string>> DeleteStudent(int id)
        {
            _logger.LogInformation("Tentando deletar estudante com ID {ID}.", id);
            var success = await _studentsService.DeleteStudentAsync(id);
            if (!success)
            {
                _logger.LogWarning("Estudante com ID {ID} não encontrado.", id);
                return NotFound($"Estudante do id {id} não encontrado.");
            }
            _logger.LogInformation("Estudante com ID {ID} deletado.", id);
            return Ok($"Estudante do id {id} deletado!");
        }
    }
}
