using System.ComponentModel.DataAnnotations;

namespace SistemaDoLeoBlazor.API.Entities
{
    public class Operador
    {
        public int id { get; set; }

        [MaxLength(50)]
        public required string nome { get; set; } = string.Empty;

        [MaxLength(4)]
        public required string senha { get; set; } = string.Empty;
        public bool admin {  get; set; }
        public bool inativo { get; set; }

        public ICollection<OperadorTela> telas { get; set; } = new List<OperadorTela>();
    }
}
