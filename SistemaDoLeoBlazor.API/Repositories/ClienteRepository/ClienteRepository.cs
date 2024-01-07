using Microsoft.EntityFrameworkCore;
using SistemaDoLeoBlazor.API.Context;
using SistemaDoLeoBlazor.API.Entities;
using SistemaDoLeoBlazor.MODELS.ClienteDTO;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Runtime.ConstrainedExecution;

namespace SistemaDoLeoBlazor.API.Repositories.ClienteRepository
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly AppDbContext _context;

        public ClienteRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Cliente> DeleteCliente(int id)
        {
            var cliente = await _context.Cliente.FindAsync(id);

            if (cliente is not null)
            {
                _context.Remove(cliente);
                await _context.SaveChangesAsync();
            }

            return cliente;
        }

        public async Task<IEnumerable<Cliente>> GetAllClientes()
        {
            return await _context.Cliente.ToListAsync();
        }

        public async Task<Cliente> GetClienteById(int id)
        {
            return await _context.Cliente.FindAsync(id);
        }

        public async Task<Cliente> PatchCliente(int id, ClienteDTO clienteDTO)
        {
            var cliente = await _context.Cliente.FindAsync(id);

            if (cliente is not null)
            {
                cliente.nome = clienteDTO.nome;
                cliente.documento = clienteDTO.documento;
                cliente.dtNasc = clienteDTO.dtNasc;
                cliente.bairro = clienteDTO.bairro;
                cliente.cep = clienteDTO.cep;
                cliente.cidade = clienteDTO.cidade;
                cliente.complemento = clienteDTO.complemento;
                cliente.endereco = clienteDTO.endereco;
                cliente.inativo = clienteDTO.inativo;
                cliente.numero = clienteDTO.numero;
                cliente.tipoCliente = clienteDTO.tipoCliente;
                cliente.tipoForncedor = clienteDTO.tipoFornecedor;
                cliente.uf = clienteDTO.uf;

                await _context.SaveChangesAsync();
            }

            return cliente;
        }

        public async Task<Cliente> PostCliente(ClienteDTO clienteDTO)
        {
            var cliente = new Cliente
            {
                nome = clienteDTO.nome,
                documento = clienteDTO.documento,
                dtNasc = clienteDTO.dtNasc,
                bairro = clienteDTO.bairro,
                cep = clienteDTO.cep,
                cidade = clienteDTO.cidade,
                complemento = clienteDTO.complemento,
                endereco = clienteDTO.endereco,
                inativo = clienteDTO.inativo,
                numero = clienteDTO.numero,
                tipoCliente = clienteDTO.tipoCliente,
                tipoForncedor = clienteDTO.tipoFornecedor,
                uf = clienteDTO.uf
            };

            var resultado = _context.Cliente.AddAsync(cliente);
            await _context.SaveChangesAsync();
            return resultado.Result.Entity;
        }
    }
}
