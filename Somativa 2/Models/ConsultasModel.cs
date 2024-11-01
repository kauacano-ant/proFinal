using System;
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

		[DisplayName("Consultório")]
		public Guid? ConsultorioId { get; set; }
		public ConsultoriosModel? Consultorio { get; set; }

		[DisplayName("Paciente")]
		public Guid? PacienteId { get; set; }
		public PacientesModel? Pacientes { get; set; }
		public string? UserId { get; set; }
	}
}
