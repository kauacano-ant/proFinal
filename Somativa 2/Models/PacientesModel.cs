using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Somativa_2.Models
{
	public class PacientesModel
	{
		[Key]
		public Guid PacienteId { get; set; }

		[DisplayName("Nome do Paciente")]
		public string Nome { get; set; }
		public string CPF { get; set; }
		public string RG { get; set; }

		[DisplayName("Data de nascimento")]
		public DateTime Data_de_nascimento { get; set; }

		[DisplayName("Endereço")]
		public string Endereco { get; set; }
		public string Telefone { get; set; }
		public string? img { get; set; }
		public string? UserId { get; set; }

		// Propriedades de navegação para o plano de saúde
		[DisplayName("Plano de Saúde")]
		public Guid PlanodeSaudeId { get; set; }
		public PlanodeSaudeModel? PlanodeSaude { get; set; }

		// Coleção de consultas associadas ao paciente
		public ICollection<ConsultasModel> Consultas { get; set; } = new List<ConsultasModel>();
	}
}
