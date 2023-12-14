 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDoLeoBlazor.MODELS.OperadorDTOs
{
    public class OperadorDTO
    {
        public int id { get; set; }
        public string? nome { get; set; }
        public string? senha { get; set; }
        public bool admin { get; set; }
        public bool inativo { get; set; }
    }
}
