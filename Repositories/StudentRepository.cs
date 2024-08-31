using MySql.Data.MySqlClient;
using System.Collections.Generic;
using APIRafael.Models;
using System;

namespace APIRafael.Repositories
{
    public class StudentRepository
    {
        private readonly string _connectionString;

        public StudentRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Student> GetStudents()
        {
            var students = new List<Student>();
            try
            {
                using var connection = new MySqlConnection(_connectionString);
                connection.Open();
                string query = "SELECT * FROM alunos";
                using var command = new MySqlCommand(query, connection);
                using var reader = command.ExecuteReader();
                while (reader.Read())
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
                // Logar a exceção e retornar um valor padrão ou re-throw
                Console.WriteLine($"Erro ao obter estudantes: {ex.Message}");
                throw;
            }
            return students;
        }

        public Student AddStudent(Student student)
        {
            try
            {
                using var connection = new MySqlConnection(_connectionString);
                connection.Open();
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

                var result = command.ExecuteScalar();
                if (result != null)
                {
                    student.ID = Convert.ToInt32(result);
                }
                else
                {
                    throw new InvalidOperationException("Failed to retrieve the new student's ID.");
                }
            }
            catch (MySqlException ex)
            {
                // Logar a exceção e re-throw ou retornar uma falha de operação
                Console.WriteLine($"Erro ao adicionar estudante: {ex.Message}");
                throw;
            }
            return student;
        }

        public Student UpdateStudent(Student student)
        {
            try
            {
                using var connection = new MySqlConnection(_connectionString);
                connection.Open();
                string query = @"
                    UPDATE alunos
                    SET nome = @name, idade = @age, nota_primeiro_semestre = @firstSemesterGrade, 
                        nota_segundo_semestre = @secondSemesterGrade, nome_professor = @professorName, 
                        numero_sala = @roomNumber
                    WHERE id = @id;";
                using var command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", student.ID);
                command.Parameters.AddWithValue("@name", student.Name);
                command.Parameters.AddWithValue("@age", student.Age);
                command.Parameters.AddWithValue("@firstSemesterGrade", student.FirstSemesterGrade);
                command.Parameters.AddWithValue("@secondSemesterGrade", student.SecondSemesterGrade);
                command.Parameters.AddWithValue("@professorName", student.ProfessorName);
                command.Parameters.AddWithValue("@roomNumber", student.RoomNumber);

                command.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                // Logar a exceção e re-throw ou retornar uma falha de operação
                Console.WriteLine($"Erro ao atualizar estudante: {ex.Message}");
                throw;
            }
            return student;
        }

        public Student? GetStudentById(int id)
        {
            Student? student = null;
            try
            {
                using var connection = new MySqlConnection(_connectionString);
                connection.Open();
                string query = "SELECT * FROM alunos WHERE id = @id";
                using var command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", id);

                using var reader = command.ExecuteReader();
                if (reader.Read())
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
                // Logar a exceção e re-throw ou retornar uma falha de operação
                Console.WriteLine($"Erro ao obter estudante: {ex.Message}");
                throw;
            }
            return student;
        }

        public bool DeleteStudent(int id)
        {
            try
            {
                using var connection = new MySqlConnection(_connectionString);
                connection.Open();
                string query = "DELETE FROM alunos WHERE id = @id;";
                using var command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", id);

                var rowsAffected = command.ExecuteNonQuery();
                return rowsAffected > 0;
            }
            catch (MySqlException ex)
            {
                // Logar a exceção e re-throw ou retornar uma falha de operação
                Console.WriteLine($"Erro ao deletar estudante: {ex.Message}");
                throw;
            }
        }
    }
}
