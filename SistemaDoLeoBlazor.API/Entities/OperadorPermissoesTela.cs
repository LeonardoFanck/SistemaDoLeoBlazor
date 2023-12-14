using System.ComponentModel.DataAnnotations;

namespace SistemaDoLeoBlazor.API.Entities
{
    public class OperadorPermissoesTela
    {
        public int id { get; set; }
         
        public int operadorTelaId { get; set; }

        public bool novo { get; set; }
        public bool editar { get; set; }
        public bool excluir { get; set; }

        public OperadorTela? operadorTela { get; set; }
    }
}
