using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Somativa_2.Models
{
    public class FeedbackModel
    {
        [Key]
        public Guid FeedbackId { get; set; }
        public string Comentario { get; set; }
        public DateTime Data {  get; set; }
        public int Nota { get; set; }
        [Range(1, 5)]
        [DisplayName("Consulta")]
        public Guid? ConsultaId { get; set; }
        public ConsultasModel? Consulta { get; set; }
    }
}
