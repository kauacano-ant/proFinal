using System;
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
        public string RG { get;set; }
        [DisplayName("Data de nascimento")]
        public DateTime Data_de_nascimento { get;set; }
        [DisplayName("Endereço")]
        public string Endereco { get;set; } 
        public string Telefone {  get; set; }
        [DisplayName("PlanodeSaude")]
        public Guid PlanodeSaudeId { get; set; }
        public PlanodeSaudeModel? PlanodeSaude { get; set; }
		public string? img { get; set; }
        public string? UserId { get; set; }
    }
}
