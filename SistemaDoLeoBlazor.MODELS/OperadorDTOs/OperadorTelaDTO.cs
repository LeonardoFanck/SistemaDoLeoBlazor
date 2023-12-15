using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDoLeoBlazor.MODELS.OperadorDTOs
{
    public class OperadorTelaDTO
    {
        public int id { get; set; }
        public int idOperador { get; set; }
        public int idTela { get; set; }
        
        public string? nome { get; set; }
        public bool ativo { get; set; }
        public bool novo { get; set; }
        public bool editar { get; set; }
        public bool excluir { get; set; }
    }
}
