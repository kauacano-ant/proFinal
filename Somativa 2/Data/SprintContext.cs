using Microsoft.EntityFrameworkCore;
using Somativa_2.Models;

namespace Somativa_2.Data
{
    public class SprintContext : DbContext
    {
        public SprintContext(DbContextOptions<SprintContext> options) : base(options) { }
        public DbSet<ConsultasModel> Consultas { get; set; }
        public DbSet<ConsultoriosModel> Consultorios { get; set; }
        public DbSet<FeedbackModel> Feedback { get; set; }
        public DbSet<PacientesModel> Paciente { get; set; }
        public DbSet<PlanodeSaudeModel> PlanodeSaude { get; set; }
        public DbSet<Categ> Categ { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ConsultasModel>().ToTable("Consultas"); ;
            modelBuilder.Entity<ConsultoriosModel>().ToTable("Consultorios");
            modelBuilder.Entity<FeedbackModel>().ToTable("feedback");
            modelBuilder.Entity<PacientesModel>().ToTable("Pacientes");
            modelBuilder.Entity<PlanodeSaudeModel>().ToTable("Planos_de_Saude");
            modelBuilder.Entity<Categ>().ToTable("Categ");
            modelBuilder.Entity<ConsultasModel>()
            .HasOne(c => c.Consultorio)
            .WithMany()
            .HasForeignKey(c => c.ConsultorioId);

            modelBuilder.Entity<ConsultasModel>()
                .HasOne(c => c.Pacientes)
                .WithMany()
                .HasForeignKey(c => c.PacienteId);
        }
    } 
}
