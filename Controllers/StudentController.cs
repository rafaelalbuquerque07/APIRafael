using APIRafael.Models;
using APIRafael.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace APIRafael.Controllers
{
    public class StudentController : Controller
    {
        private readonly StudentsService _studentsService;
        private readonly ILogger<StudentController> _logger;

        public StudentController(StudentsService studentsService, ILogger<StudentController> logger)
        {
            _studentsService = studentsService;
            _logger = logger;
        }

        [HttpGet("StudentList")]
        public async Task<IActionResult> StudentList()
        {
            _logger.LogInformation("Obtendo a lista de estudantes.");
            var students = await _studentsService.GetAllStudentsAsync();
            return View(students); // Passa a lista de estudantes para a view
        }
    }
}
