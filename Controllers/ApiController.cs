using APIRafael.Models;
using APIRafael.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System;

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
        public async Task<IActionResult> AddStudent([FromBody] Student student)
        {
            if (student == null)
            {
                _logger.LogWarning("O estudante recebido é nulo.");
                return BadRequest("O estudante não pode ser nulo.");
            }

            // Verifica se os valores do estudante são válidos
            if (student.ID == 0 ||
                string.IsNullOrWhiteSpace(student.Name) ||
                student.Age <= 0 ||
                student.FirstSemesterGrade <= 0 ||
                student.SecondSemesterGrade <= 0 ||
                string.IsNullOrWhiteSpace(student.ProfessorName) ||
                student.RoomNumber <= 0)
            {
                _logger.LogWarning("Os valores fornecidos são inválidos. Todos os campos devem ser diferentes dos valores padrão.");
                return BadRequest("Os valores fornecidos são inválidos. Todos os campos devem ser diferentes dos valores padrão.");
            }

            try
            {
                await _studentsService.AddStudentAsync(student);
                return CreatedAtAction(nameof(GetStudentById), new { id = student.ID }, student);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Erro ao adicionar o estudante. Pode ser um ID duplicado.");
                return Conflict("Erro ao adicionar o estudante. Pode ser um ID duplicado.");
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Erro ao adicionar o estudante. Operação inválida.");
                return StatusCode(404, "Erro de operação inválida ao tentar adicionar o estudante.");
            }
        }

        [HttpPut(Name = "UpdateStudent")]
        public async Task<IActionResult> UpdateStudent([FromBody] Student student)
        {
            if (student == null)
            {
                _logger.LogWarning("O estudante recebido é nulo.");
                return BadRequest("O estudante não pode ser nulo.");
            }

            // Verifica se os valores do estudante são válidos
            if (student.ID == 0 ||
                string.IsNullOrWhiteSpace(student.Name) ||
                student.Age <= 0 ||
                student.FirstSemesterGrade <= 0 ||
                student.SecondSemesterGrade <= 0 ||
                string.IsNullOrWhiteSpace(student.ProfessorName) ||
                student.RoomNumber <= 0)
            {
                _logger.LogWarning("Os valores fornecidos são inválidos. Todos os campos devem ser diferentes dos valores padrão.");
                return BadRequest("Os valores fornecidos são inválidos. Todos os campos devem ser diferentes dos valores padrão.");
            }

            try
            {
                var updatedStudent = await _studentsService.UpdateStudentAsync(student.ID, student);
                if (updatedStudent == null)
                {
                    _logger.LogWarning("Estudante com ID {ID} não encontrado.", student.ID);
                    return NotFound($"Estudante com ID {student.ID} não encontrado.");
                }
                return Ok($"Estudante {updatedStudent.Name} atualizado!");
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Erro ao atualizar o estudante.");
                return Conflict("Erro ao atualizar o estudante. Pode haver um problema com os dados fornecidos.");
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Erro ao atualizar o estudante. Operação inválida.");
                return StatusCode(404, "Erro de operação inválida ao tentar atualizar o estudante.");
            }
        }

        [HttpGet("{id}", Name = "GetStudentById")]
        public async Task<ActionResult<Student>> GetStudentById(int id)
        {
            _logger.LogInformation("Obtendo estudante com ID {ID}.", id);
            var student = await _studentsService.GetStudentByIdAsync(id);
            if (student == null)
            {
                _logger.LogWarning("Estudante com ID {ID} não encontrado.", id);
                return NotFound($"Estudante do id {id} não encontrado."); // Retorna 404 com a mensagem personalizada
            }

            _logger.LogInformation("Estudante {Name} encontrado com sucesso!", student.Name);
            return Ok(student);
        }


        [HttpDelete(Name = "DeleteStudent")]
        public async Task<ActionResult<string>> DeleteStudent(int id)
        {
            _logger.LogInformation("Tentando deletar estudante com ID {ID}.", id);

            // Obtém o estudante para verificar o nome
            var student = await _studentsService.GetStudentByIdAsync(id);
            if (student == null)
            {
                _logger.LogWarning("Estudante com ID {ID} não encontrado.", id);
                return NotFound($"Estudante com ID {id} não encontrado.");
            }

            // Deleta o estudante
            var success = await _studentsService.DeleteStudentAsync(id);
            if (!success)
            {
                _logger.LogError("Falha ao deletar estudante com ID {ID}.", id);
                return StatusCode(500, "Erro ao deletar o estudante.");
            }

            _logger.LogInformation("Estudante {Name} com ID {ID} deletado.", student.Name, id);
            return Ok($"Estudante {student.Name} com ID {id} deletado!");
        }

    }
}
