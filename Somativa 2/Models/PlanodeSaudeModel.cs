using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        public string? img {  get; set; }
    
    

    }
}
