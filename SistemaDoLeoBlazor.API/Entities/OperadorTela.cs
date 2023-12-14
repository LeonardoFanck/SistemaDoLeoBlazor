using System.ComponentModel.DataAnnotations;

namespace SistemaDoLeoBlazor.API.Entities
{
    public class OperadorTela
    {
        public int id { get; set; }

        public int operadorId { get; set; }

        [MaxLength(50)]
        public string nome { get; set; } = string.Empty;
        public bool ativo { get; set; }

        
        public Operador? operador { get; set; }
        public OperadorPermissoesTela? operadorPermissoesTela { get; set; }
    }
}
