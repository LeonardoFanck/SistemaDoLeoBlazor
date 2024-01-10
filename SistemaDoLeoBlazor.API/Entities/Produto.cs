using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaDoLeoBlazor.API.Entities
{
    public class Produto
    {
        public int id { get; set; }
        public required int categoriaId { get; set; }

        [MaxLength(50)]
        public required string nome { get; set; } = string.Empty;
        [Column(TypeName = "decimal(10, 2)")]
        public required decimal preco { get; set; } = decimal.Zero;
        [Column(TypeName = "decimal(10, 2)")]
        public required decimal custo { get; set; } = decimal.Zero;
        [MaxLength(4)]
        public required string unidade { get; set; } = string.Empty;
        public long estoque { get; set; } = 0;
        public bool inativo { get; set; } = false;

        public Categoria? categoria { get; set;}
        public ICollection<PedidoItem> pedidoItens = new List<PedidoItem>();
    }
}
