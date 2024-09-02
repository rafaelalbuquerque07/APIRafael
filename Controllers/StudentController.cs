using APIRafael.Models;
using APIRafael.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace APIRafael.Controllers
{
    public class StudentController : Controller
    {
        private readonly StudentRepository _studentRepository;
        private readonly ILogger<StudentController> _logger;

        public StudentController(StudentRepository studentRepository, ILogger<StudentController> logger)
        {
            _studentRepository = studentRepository;
            _logger = logger;
        }

        [HttpGet("StudentList")]
        public async Task<IActionResult> StudentList()
        {
            _logger.LogInformation("Obtendo a lista de estudantes.");
            var students = await _studentRepository.GetStudentsAsync();
            return View(students); // Passa a lista de estudantes para a view
        }
    }
}
