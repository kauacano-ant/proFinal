using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Somativa_2.Models
{
    public class PlanodeSaudeModel
    {
        [Key]
        public Guid PlanodeSaudeId { get; set; }
        public string Nome { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public string CNPJ { get; set; }
        public string? img { get; set; }

        // Coleção de pacientes associados ao plano de saúde
        public ICollection<PacientesModel> Pacientes { get; set; } = new List<PacientesModel>();
    }
}
