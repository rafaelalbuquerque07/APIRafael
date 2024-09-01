using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;
using APIRafael.Models;
using Microsoft.Extensions.Logging;

namespace APIRafael.Repositories
{
    public class StudentRepository
    {
        private readonly string _connectionString;
        private readonly ILogger<StudentRepository> _logger;

        public StudentRepository(string connectionString, ILogger<StudentRepository> logger)
        {
            _connectionString = connectionString;
            _logger = logger;
        }

        public async Task<List<Student>> GetStudentsAsync()
        {
            var students = new List<Student>();
            try
            {
                using var connection = new MySqlConnection(_connectionString);
                await connection.OpenAsync();
                string query = "SELECT * FROM alunos";
                using var command = new MySqlCommand(query, connection);
                using var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    students.Add(new Student
                    {
                        ID = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Age = reader.GetInt32(2),
                        FirstSemesterGrade = Convert.ToDouble(reader.GetDecimal(3)),
                        SecondSemesterGrade = Convert.ToDouble(reader.GetDecimal(4)),
                        ProfessorName = reader.GetString(5),
                        RoomNumber = reader.GetInt32(6)
                    });
                }
            }
            catch (MySqlException ex)
            {
                _logger.LogError("Erro ao obter lista de estudantes: {Message}", ex.Message);
                throw;
            }
            return students;
        }

        public async Task<Student> AddStudentAsync(Student student)
        {
            try
            {
                using var connection = new MySqlConnection(_connectionString);
                await connection.OpenAsync();
                string query = @"
                    INSERT INTO alunos (nome, idade, nota_primeiro_semestre, nota_segundo_semestre, nome_professor, numero_sala)
                    VALUES (@name, @age, @firstSemesterGrade, @secondSemesterGrade, @professorName, @roomNumber);
                    SELECT LAST_INSERT_ID();";
                using var command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@name", student.Name);
                command.Parameters.AddWithValue("@age", student.Age);
                command.Parameters.AddWithValue("@firstSemesterGrade", student.FirstSemesterGrade);
                command.Parameters.AddWithValue("@secondSemesterGrade", student.SecondSemesterGrade);
                command.Parameters.AddWithValue("@professorName", student.ProfessorName);
                command.Parameters.AddWithValue("@roomNumber", student.RoomNumber);

                var result = await command.ExecuteScalarAsync();
                if (result != null)
                {
                    student.ID = Convert.ToInt32(result);
                }
                else
                {
                    throw new InvalidOperationException("Falha ao recuperar o ID do novo estudante.");
                }
            }
            catch (MySqlException ex)
            {
                _logger.LogError("Erro ao adicionar estudante: {Message}", ex.Message);
                throw;
            }
            return student;
        }

        public async Task<Student> UpdateStudentAsync(int id, Student student)
        {
            try
            {
                using var connection = new MySqlConnection(_connectionString);
                await connection.OpenAsync();
                string query = @"
                    UPDATE alunos
                    SET nome = @name, idade = @age, nota_primeiro_semestre = @firstSemesterGrade, 
                        nota_segundo_semestre = @secondSemesterGrade, nome_professor = @professorName, 
                        numero_sala = @roomNumber
                    WHERE id = @id;";
                using var command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@name", student.Name);
                command.Parameters.AddWithValue("@age", student.Age);
                command.Parameters.AddWithValue("@firstSemesterGrade", student.FirstSemesterGrade);
                command.Parameters.AddWithValue("@secondSemesterGrade", student.SecondSemesterGrade);
                command.Parameters.AddWithValue("@professorName", student.ProfessorName);
                command.Parameters.AddWithValue("@roomNumber", student.RoomNumber);

                await command.ExecuteNonQueryAsync();
            }
            catch (MySqlException ex)
            {
                _logger.LogError("Erro ao atualizar estudante com ID {Id}: {Message}", id, ex.Message);
                throw;
            }
            return student;
        }

        public async Task<Student?> GetStudentByIdAsync(int id)
        {
            Student? student = null;
            try
            {
                using var connection = new MySqlConnection(_connectionString);
                await connection.OpenAsync();
                string query = "SELECT * FROM alunos WHERE id = @id";
                using var command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", id);

                using var reader = await command.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    student = new Student
                    {
                        ID = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Age = reader.GetInt32(2),
                        FirstSemesterGrade = Convert.ToDouble(reader.GetDecimal(3)),
                        SecondSemesterGrade = Convert.ToDouble(reader.GetDecimal(4)),
                        ProfessorName = reader.GetString(5),
                        RoomNumber = reader.GetInt32(6)
                    };
                }
            }
            catch (MySqlException ex)
            {
                _logger.LogError("Erro ao obter estudante com ID {Id}: {Message}", id, ex.Message);
                throw;
            }
            return student;
        }

        public async Task<bool> DeleteStudentAsync(int id)
        {
            try
            {
                using var connection = new MySqlConnection(_connectionString);
                await connection.OpenAsync();
                string query = "DELETE FROM alunos WHERE id = @id;";
                using var command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", id);

                var rowsAffected = await command.ExecuteNonQueryAsync();
                return rowsAffected > 0;
            }
            catch (MySqlException ex)
            {
                _logger.LogError("Erro ao deletar estudante com ID {Id}: {Message}", id, ex.Message);
                throw;
            }
        }
    }
}
