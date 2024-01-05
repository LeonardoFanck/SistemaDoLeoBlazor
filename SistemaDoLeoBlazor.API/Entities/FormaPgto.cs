using System.ComponentModel.DataAnnotations;

namespace SistemaDoLeoBlazor.API.Entities
{
    public class FormaPgto
    {
        public int id { get; set; }
        [MaxLength(50)]
        public required string nome {  get; set; } = string.Empty;
        public bool inativo { get; set; } = false;

        public Pedido? pedido;
    }
}
