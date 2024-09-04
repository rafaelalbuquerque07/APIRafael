using Microsoft.EntityFrameworkCore;
using APIRafael.Data;
using APIRafael.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace APIRafael.Services
{
    public class StudentsService
    {
        private readonly ApplicationDbContext _context;

        public StudentsService(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        // Corrigido para o nome correto do método GetAllStudentsAsync
        public async Task<List<Student>> GetAllStudentsAsync()
        {
            // Garantir que _context e _context.Students não sejam nulos
            if (_context == null || _context.Students == null)
            {
                throw new InvalidOperationException("Contexto ou DbSet de estudantes não foram inicializados.");
            }

            return await _context.Students.ToListAsync();
        }

        public async Task<Student?> GetStudentByIdAsync(int id)
        {
            // Garantir que _context e _context.Students não sejam nulos
            if (_context == null || _context.Students == null)
            {
                throw new InvalidOperationException("Contexto ou DbSet de estudantes não foram inicializados.");
            }

            return await _context.Students.SingleOrDefaultAsync(s => s.ID == id);
        }

        public async Task<Student?> AddStudentAsync(Student student)
        {
            if (_context == null || _context.Students == null)
            {
                throw new InvalidOperationException("Contexto ou DbSet de estudantes não foram inicializados.");
            }

            if (student == null)
            {
                throw new ArgumentNullException(nameof(student), "O estudante não pode ser nulo.");
            }

            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            return student;
        }

        public async Task<Student?> UpdateStudentAsync(int id, Student student)
        {
            if (_context == null || _context.Students == null)
            {
                throw new InvalidOperationException("Contexto ou DbSet de estudantes não foram inicializados.");
            }

            if (student == null || student.ID != id)
            {
                throw new ArgumentException("Dados do estudante são inválidos.");
            }

            var existingStudent = await _context.Students.FindAsync(id);

            if (existingStudent == null)
            {
                return null;
            }

            existingStudent.Name = student.Name;
            existingStudent.Age = student.Age;
            existingStudent.FirstSemesterGrade = student.FirstSemesterGrade;
            existingStudent.SecondSemesterGrade = student.SecondSemesterGrade;
            existingStudent.ProfessorName = student.ProfessorName;
            existingStudent.RoomNumber = student.RoomNumber;

            await _context.SaveChangesAsync();

            return existingStudent;
        }

        public async Task<bool> DeleteStudentAsync(int id)
        {
            if (_context == null || _context.Students == null)
            {
                throw new InvalidOperationException("Contexto ou DbSet de estudantes não foram inicializados.");
            }

            var student = await _context.Students.FindAsync(id);

            if (student == null)
            {
                return false;
            }

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
