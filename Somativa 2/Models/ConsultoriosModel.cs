using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Somativa_2.Models
{
    public class ConsultoriosModel
    {
        [Key]
        public Guid ConsultorioId { get; set; }
        [DisplayName("Nome do Consultorio")]
        public string Nome { get; set; }
        [DisplayName("Endereço")]
        public string Endereco { get; set;}
        public string Telefone { get; set; }
        public string Email { get; set; }
        public string Especialidade { get; set; }
        public Guid CategId { get; set; }
        public Categ? Categ { get; set; }
        public string? img { get; set; }
        [DisplayName("Descrição")]
        public string Descricao { get; set; }
    }
}