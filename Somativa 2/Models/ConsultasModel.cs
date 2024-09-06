using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Somativa_2.Models
{
    public class ConsultasModel
    {
        [Key]
        public Guid ConsultaId { get; set; }
        public DateTime DataConsultas { get; set; }    
        public DateTime Hora { get; set; }
        [DisplayName("Consultorio")]
        public Guid ConsultorioId { get; set; }
        public ConsultoriosModel? Consultorio { get; set; }
        [DisplayName("Pacientes")]
        public Guid PacienteId { get; set; }
        public PacientesModel? Pacientes { get; set; }
    }
}
