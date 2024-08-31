namespace APIRafael.Models
{
    public class Student
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public double FirstSemesterGrade { get; set; }
        public double SecondSemesterGrade { get; set; }
        public string ProfessorName { get; set; }
        public int RoomNumber { get; set; }

        // Construtor que inicializa todas as propriedades não anuláveis
        public Student(string name, string professorName)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            ProfessorName = professorName ?? throw new ArgumentNullException(nameof(professorName));
        }

        // Construtor padrão
        public Student()
        {
            // Inicialize as propriedades não anuláveis com valores padrão
            Name = string.Empty;
            ProfessorName = string.Empty;
        }
    }
}
