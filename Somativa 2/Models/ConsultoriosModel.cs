using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Somativa_2.Models
{
    public class ConsultoriosModel
    {
        [Key]
        public Guid ConsultorioId { get; set; }
        public string Nome { get; set; }
        [DisplayName("Endereço")]
        public string Endereco { get; set;}
        public string Telefone { get; set; }
        public string Email { get; set; }

        public string Especialidade { get; set; }
    }
}
