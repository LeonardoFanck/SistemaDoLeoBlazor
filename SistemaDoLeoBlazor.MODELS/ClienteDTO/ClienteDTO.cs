using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDoLeoBlazor.MODELS.ClienteDTO
{
    public class ClienteDTO
    {
        public int id { get; set; }
        public string? nome { get; set; }
        public string? documento {  get; set; }
        public DateTime dtNasc { get; set; }
        public bool inativo { get; set; }
        public string? cep { get; set; }
        public string? uf { get; set; }
        public string? cidade { get; set; }
        public string? bairro { get; set; }
        public string? endereco { get; set; }
        public string? numero { get; set; }
        public string? complemento { get; set; }
        public bool tipoCliente { get; set; }
        public bool tipoFornecedor { get; set; }
    }
}
