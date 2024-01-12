using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaDoLeoBlazor.API.Entities
{
    public class Pedido
    {
        public int id { get; set; }
        public required int clienteId { get; set; }
        public required int formaPgtoId { get; set; }
        [Column(TypeName = "date")]
        public required DateTime data { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public required decimal valor {  get; set; } = decimal.Zero;
        [Column(TypeName = "decimal(10, 2)")]
        public required decimal desconto { get; set; } = decimal.Zero;
        [Column(TypeName = "decimal(10, 2)")]
        public required decimal total { get; set; } = decimal.Zero;
        public required string tipoOperacao { get; set; } = string.Empty;

        public Cliente? cliente { get; set; }
        public FormaPgto? formaPgto { get; set; }
        public ICollection<PedidoItem> pedidoItens = new List<PedidoItem>();
    }
}
