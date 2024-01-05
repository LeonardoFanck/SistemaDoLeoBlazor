using System.ComponentModel.DataAnnotations;

namespace SistemaDoLeoBlazor.API.Entities
{
    public class Tela
    {
        public int id { get; set; }

        [MaxLength(100)]
        public required string nome { get; set; }


        public ICollection<OperadorTela> operadorTelas = new List<OperadorTela>();
    }
}
