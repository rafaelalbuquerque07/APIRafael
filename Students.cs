using APIRafael.Models;
using MySqlConnector;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace APIRafael
{
    public static class Students
    {
        // Torna o campo readonly e simplifica a inicialização da conexão
        private static readonly string _connectionString = "server=bcj58eqzeozhmpxgno7s-mysql.services.clever-cloud.com;port=3306;user=u2njneaa2yl7k94v;database=bcj58eqzeozhmpxgno7s;password=qOBXw7eQ0QasDnCqGEzq;SslMode=Required";

        public static async Task<List<Student>> GetStudentsAsync()
        {
            var students = new List<Student>();

            await using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();

            string query = "SELECT * FROM Students";
            await using var command = new MySqlCommand(query, connection);
            await using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                students.Add(new Student
                {
                    ID = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Age = reader.GetInt32(2),
                    FirstSemesterGrade = reader.GetDouble(3),
                    SecondSemesterGrade = reader.GetDouble(4),
                    ProfessorName = reader.GetString(5),
                    RoomNumber = reader.GetInt32(6)
                });
            }

            return students;
        }

        public static async Task<Student> AddStudentAsync(Student student)
        {
            await using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();

            string query = @"
                INSERT INTO Students (Name, Age, FirstSemesterGrade, SecondSemesterGrade, ProfessorName, RoomNumber)
                VALUES (@name, @age, @firstSemesterGrade, @secondSemesterGrade, @professorName, @roomNumber);
                SELECT LAST_INSERT_ID();";

            await using var command = new MySqlCommand(query, connection);
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

            return student;
        }

        public static async Task<Student> UpdateStudentAsync(int id, Student student)
        {
            await using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();

            string query = @"
                UPDATE Students
                SET Name = @name, Age = @age, FirstSemesterGrade = @firstSemesterGrade, 
                    SecondSemesterGrade = @secondSemesterGrade, ProfessorName = @professorName, 
                    RoomNumber = @roomNumber
                WHERE id = @id;";

            await using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@id", id);
            command.Parameters.AddWithValue("@name", student.Name);
            command.Parameters.AddWithValue("@age", student.Age);
            command.Parameters.AddWithValue("@firstSemesterGrade", student.FirstSemesterGrade);
            command.Parameters.AddWithValue("@secondSemesterGrade", student.SecondSemesterGrade);
            command.Parameters.AddWithValue("@professorName", student.ProfessorName);
            command.Parameters.AddWithValue("@roomNumber", student.RoomNumber);

            await command.ExecuteNonQueryAsync();

            return student;
        }

        public static async Task<Student?> GetStudentByIdAsync(int id)
        {
            Student? student = null;

            await using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();

            string query = "SELECT * FROM Students WHERE id = @id";
            await using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@id", id);

            await using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                student = new Student
                {
                    ID = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Age = reader.GetInt32(2),
                    FirstSemesterGrade = reader.GetDouble(3),
                    SecondSemesterGrade = reader.GetDouble(4),
                    ProfessorName = reader.GetString(5),
                    RoomNumber = reader.GetInt32(6)
                };
            }

            return student;
        }

        public static async Task<bool> DeleteStudentAsync(int id)
        {
            await using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();

            string query = "DELETE FROM Students WHERE id = @id";
            await using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@id", id);

            int rowsAffected = await command.ExecuteNonQueryAsync();

            return rowsAffected > 0;
        }
    }
}
