using System.ComponentModel.DataAnnotations;

namespace SistemaDoLeoBlazor.API.Entities
{
    public class Tela
    {
        public int Id { get; set; }

        [MaxLength(100)]
        public required string Nome { get; set; }


        public ICollection<OperadorTela> operadorTelas = new List<OperadorTela>();
    }
}
