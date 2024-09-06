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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ConsultasModel>().ToTable("Consultas"); ;
            modelBuilder.Entity<ConsultoriosModel>().ToTable("Consultorios");
            modelBuilder.Entity<FeedbackModel>().ToTable("feedback");
            modelBuilder.Entity<PacientesModel>().ToTable("Pacientes");
            modelBuilder.Entity<PlanodeSaudeModel>().ToTable("Planos_de_Saude");
        }
    } 
}
