using Microsoft.EntityFrameworkCore;
using APIRafael.Models;

namespace APIRafael.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Marque a propriedade como anulável
        public DbSet<Student>? Students { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuração da tabela e das propriedades
            modelBuilder.Entity<Student>(entity =>
            {
                entity.ToTable("Students");

                entity.HasKey(e => e.ID);

                entity.Property(e => e.ID)
                    .HasColumnName("id")
                    .IsRequired();

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(255)
                    .IsRequired();

                entity.Property(e => e.Age)
                    .HasColumnName("age")
                    .IsRequired();

                entity.Property(e => e.FirstSemesterGrade)
                    .HasColumnName("firstSemesterGrade")
                    .HasColumnType("double")
                    .IsRequired();

                entity.Property(e => e.SecondSemesterGrade)
                    .HasColumnName("secondSemesterGrade")
                    .HasColumnType("double")
                    .IsRequired();

                entity.Property(e => e.ProfessorName)
                    .HasColumnName("professorName")
                    .HasMaxLength(255)
                    .IsRequired();

                entity.Property(e => e.RoomNumber)
                    .HasColumnName("roomNumber")
                    .IsRequired();
            });
        }
    }
}
