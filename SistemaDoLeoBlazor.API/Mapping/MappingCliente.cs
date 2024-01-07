using SistemaDoLeoBlazor.API.Entities;
using SistemaDoLeoBlazor.MODELS.ClienteDTO;

namespace SistemaDoLeoBlazor.API.Mapping
{
    public static class MappingCliente
    {
        public static ClienteDTO ClienteToDto(this Cliente cliente)
        {
            return new ClienteDTO
            {
                id = cliente.id,
                nome = cliente.nome,
                documento = cliente.documento,
                dtNasc = cliente.dtNasc,
                bairro = cliente.bairro,
                cep = cliente.cep,
                cidade = cliente.cidade,
                complemento = cliente.complemento,
                endereco = cliente.endereco,
                inativo = cliente.inativo,
                numero = cliente.numero,
                tipoCliente = cliente.tipoCliente,
                tipoFornecedor = cliente.tipoForncedor,
                uf = cliente.uf
            };
        }

        public static IEnumerable<ClienteDTO> ClientesToDto(this IEnumerable<Cliente> clientes)
        {
            return (from cliente in clientes
                    select new ClienteDTO
                    {
                        id = cliente.id,
                        nome = cliente.nome,
                        documento = cliente.documento,
                        dtNasc = cliente.dtNasc,
                        bairro = cliente.bairro,
                        cep = cliente.cep,
                        cidade = cliente.cidade,
                        complemento = cliente.complemento,
                        endereco = cliente.endereco,
                        inativo = cliente.inativo,
                        numero = cliente.numero,
                        tipoCliente = cliente.tipoCliente,
                        tipoFornecedor = cliente.tipoForncedor,
                        uf = cliente.uf
                    });
        }
    }
}
