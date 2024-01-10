using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDoLeoBlazor.MODELS.ProdutoDTO
{
    public class ProdutoDTO
    {
        public int id {  get; set; }
        public int categoriaId { get; set; }
        public string? nome { get; set; }
        public decimal preco { get; set; }
        public decimal custo { get; set; }
        public string? unidade { get; set; }
        public long estoque { get; set; }
        public bool inativo { get; set; }

    }
}
