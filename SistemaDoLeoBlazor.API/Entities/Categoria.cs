using System.ComponentModel.DataAnnotations;

namespace SistemaDoLeoBlazor.API.Entities
{
    public class Categoria
    {
        public int id { get; set; }
        [MaxLength(50)]
        public required string nome { get; set; }   
        public bool inativo { get; set; }

        public ICollection<Produto> produtos = new List<Produto>();
    }
}
