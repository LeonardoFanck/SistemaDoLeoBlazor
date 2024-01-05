using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaDoLeoBlazor.API.Entities
{
    public class Cliente
    {
        public int id { get; set; }
        [MaxLength(70)]
        public required string nome { get; set; } = string.Empty;
        [MaxLength(18)]
        public required string documento { get; set; } = string.Empty;
        [Column(TypeName = "date")]
        public DateTime dtNasc { get; set; }
        public bool inativo { get; set; } = false;
        [MaxLength(9)]
        public required string cep { get; set; } = string.Empty;
        [MaxLength(2)]
        public required string uf { get; set;} = string.Empty;
        [MaxLength(50)]
        public required string cidade { get; set; } = string.Empty;
        [MaxLength(50)]
        public required string bairro { get; set; } = string.Empty;
        [MaxLength(50)]
        public required string endereco { get; set; } = string.Empty;
        [MaxLength(8)]
        public required string numero { get; set; } = string.Empty;
        [MaxLength(50)]
        public required string complemento { get; set; } = string.Empty;
        public bool tipoCliente { get; set; } = false;
        public bool tipoForncedor { get; set; } = false;

        public Pedido? pedido;
    }
}
