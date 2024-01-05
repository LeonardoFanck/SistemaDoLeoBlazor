using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDoLeoBlazor.MODELS.PedidoDTO
{
    public class PedidoItemDTO
    {
        public int id {  get; set; }
        public int pedidoId { get; set; }
        public int produtoId { get; set; }
        public string? produtoNome { get; set; }
        public decimal valor {  get; set; }
        public int quantidade { get; set; }
        public decimal desconto { get; set; }
        public decimal total { get; set; }
    }
}
