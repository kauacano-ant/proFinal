using System.ComponentModel.DataAnnotations;

namespace Somativa_2.Models
{
    public class Categ
    {
        public Guid CategId { get; set; }
        [Required(ErrorMessage = "O Campo Nome é obrigatório.")]
        public string Nome { get; set; }
        public IEnumerable<ConsultoriosModel>? Consultorios { get; set; }
    }
}