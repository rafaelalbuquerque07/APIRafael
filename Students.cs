using APIRafael.Models;
using MySqlConnector;
using System.Collections.Generic;

namespace APIRafael
{
    public static class Students
    {
        // Torna o campo somente leitura
        private static readonly string connectionString = "server=localhost;user=root;database=banco;password=teste";

        public static List<Student> GetStudents()
        {
            var students = new List<Student>(); // Expressão 'new' simplificada
            using var connection = new MySqlConnection(connectionString); // Instrução 'using' simplificada
            connection.Open();
            string query = "SELECT * FROM alunos";
            using var command = new MySqlCommand(query, connection); // Instrução 'using' simplificada
            using var reader = command.ExecuteReader(); // Instrução 'using' simplificada

            while (reader.Read())
            {
                var student = new Student
                {
                    ID = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Age = reader.GetInt32(2),
                    FirstSemesterGrade = (double)reader.GetDecimal(3), // Conversão explícita de decimal para double
                    SecondSemesterGrade = (double)reader.GetDecimal(4), // Conversão explícita de decimal para double
                    ProfessorName = reader.GetString(5),
                    RoomNumber = reader.GetInt32(6)
                };
                students.Add(student);
            }

            return students;
        }

        public static Student AddStudent(Student student)
        {
            using var connection = new MySqlConnection(connectionString); // Instrução 'using' simplificada
            connection.Open();
            string query = "INSERT INTO alunos (nome, idade, nota_1, nota_2, professor, sala) VALUES (@name, @age, @firstSemesterGrade, @secondSemesterGrade, @professorName, @roomNumber)";
            using var command = new MySqlCommand(query, connection); // Instrução 'using' simplificada
            command.Parameters.AddWithValue("@name", student.Name);
            command.Parameters.AddWithValue("@age", student.Age);
            command.Parameters.AddWithValue("@firstSemesterGrade", (decimal)student.FirstSemesterGrade); // Conversão explícita de double para decimal
            command.Parameters.AddWithValue("@secondSemesterGrade", (decimal)student.SecondSemesterGrade); // Conversão explícita de double para decimal
            command.Parameters.AddWithValue("@professorName", student.ProfessorName);
            command.Parameters.AddWithValue("@roomNumber", student.RoomNumber);
            command.ExecuteNonQuery();

            return student;
        }

        public static Student UpdateStudent(int id, Student student)
        {
            using var connection = new MySqlConnection(connectionString); // Instrução 'using' simplificada
            connection.Open();
            string query = "UPDATE alunos SET nome = @name, idade = @age, nota_1 = @firstSemesterGrade, nota_2 = @secondSemesterGrade, professor = @professorName, sala = @roomNumber WHERE id = @id";
            using var command = new MySqlCommand(query, connection); // Instrução 'using' simplificada
            command.Parameters.AddWithValue("@id", id);
            command.Parameters.AddWithValue("@name", student.Name);
            command.Parameters.AddWithValue("@age", student.Age);
            command.Parameters.AddWithValue("@firstSemesterGrade", (decimal)student.FirstSemesterGrade); // Conversão explícita de double para decimal
            command.Parameters.AddWithValue("@secondSemesterGrade", (decimal)student.SecondSemesterGrade); // Conversão explícita de double para decimal
            command.Parameters.AddWithValue("@professorName", student.ProfessorName);
            command.Parameters.AddWithValue("@roomNumber", student.RoomNumber);
            command.ExecuteNonQuery();

            return student;
        }

        public static Student? GetStudentById(int id)
        {
            Student? student = null;
            using var connection = new MySqlConnection(connectionString); // Instrução 'using' simplificada
            connection.Open();
            string query = "SELECT * FROM alunos WHERE id = @id";
            using var command = new MySqlCommand(query, connection); // Instrução 'using' simplificada
            command.Parameters.AddWithValue("@id", id);

            using var reader = command.ExecuteReader(); // Instrução 'using' simplificada
            if (reader.Read())
            {
                student = new Student
                {
                    ID = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Age = reader.GetInt32(2),
                    FirstSemesterGrade = (double)reader.GetDecimal(3), // Conversão explícita de decimal para double
                    SecondSemesterGrade = (double)reader.GetDecimal(4), // Conversão explícita de decimal para double
                    ProfessorName = reader.GetString(5),
                    RoomNumber = reader.GetInt32(6)
                };
            }

            return student;
        }

        public static bool DeleteStudent(int id)
        {
            using var connection = new MySqlConnection(connectionString); // Instrução 'using' simplificada
            connection.Open();
            string query = "DELETE FROM alunos WHERE id = @id";
            using var command = new MySqlCommand(query, connection); // Instrução 'using' simplificada
            command.Parameters.AddWithValue("@id", id);

            int rowsAffected = command.ExecuteNonQuery();

            return rowsAffected > 0;
        }
    }
}
