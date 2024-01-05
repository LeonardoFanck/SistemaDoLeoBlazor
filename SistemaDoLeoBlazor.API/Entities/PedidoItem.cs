namespace SistemaDoLeoBlazor.API.Entities
{
    public class PedidoItem
    {
        public int id { get; set; }
        public required int pedidoId { get; set; }
        public required int produtoId { get; set; }
        public required decimal valor { get; set; }
        public required int quantidade { get; set; }
        public required decimal desconto { get; set; }
        public required decimal total { get; set; }

        public Pedido? pedido { get; set; }
        public Produto? produto { get; set; }
    }
}
