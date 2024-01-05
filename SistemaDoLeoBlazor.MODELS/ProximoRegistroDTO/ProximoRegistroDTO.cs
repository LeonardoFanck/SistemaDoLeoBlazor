using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDoLeoBlazor.MODELS.ProximoRegistroDTO
{
    public class ProximoRegistroDTO
    {
        public int id { get; set; }
        public int categoria { get; set; }
        public int cliente { get; set; }
        public int formaPgto { get; set; }
        public int operador { get; set; }
        public int pedido { get; set; }
        public int produto { get; set; }
    }
}
