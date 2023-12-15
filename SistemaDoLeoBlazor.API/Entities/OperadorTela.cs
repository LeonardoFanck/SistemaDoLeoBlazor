using System.ComponentModel.DataAnnotations;

namespace SistemaDoLeoBlazor.API.Entities
{
    public class OperadorTela
    {
        public int id { get; set; }

        public int operadorId { get; set; }
        public int telaId { get; set; }

        
        public bool ativo { get; set; }
        public bool novo { get; set; }
        public bool editar { get; set; }
        public bool excluir { get; set; }


        public Operador? operador { get; set; }
        public Tela tela { get; set; }
    }
}
