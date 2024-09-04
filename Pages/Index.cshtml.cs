using Microsoft.AspNetCore.Mvc.RazorPages;
using APIRafael.Services; // Certifique-se de que o namespace do seu serviço está correto
using APIRafael.Models; // Certifique-se de que o namespace do seu modelo está correto
using System.Collections.Generic;
using System.Threading.Tasks;

namespace APIRafael.Pages
{
    public class IndexModel : PageModel
    {
        private readonly StudentsService _studentsService;

        public IndexModel(StudentsService studentsService)
        {
            _studentsService = studentsService;
            Students = new List<Student>(); // Inicializa a propriedade
        }

        public IList<Student> Students { get; set; }

        public async Task OnGetAsync()
        {
            Students = await _studentsService.GetAllStudentsAsync();
        }
    }
}
