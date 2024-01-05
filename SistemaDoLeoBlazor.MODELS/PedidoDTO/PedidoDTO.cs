using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDoLeoBlazor.MODELS.PedidoDTO
{
    public class PedidoDTO
    {
        public int id { get; set; }
        public int clienteId { get; set; }
        public string? clienteNome { get; set; }
        public int formaPgtoId { get; set; }
        public string? formaPgtoNome { get; set; }
        public DateTime data {  get; set; }
        public decimal valor { get; set; }
        public decimal desconto { get; set; }
        public decimal total { get; set; }
    }
}
