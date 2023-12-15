using Microsoft.EntityFrameworkCore;
using SistemaDoLeoBlazor.API.Context;
using SistemaDoLeoBlazor.API.Entities;
using SistemaDoLeoBlazor.MODELS.OperadorDTOs;

namespace SistemaDoLeoBlazor.API.Repositories.OperadorRepository
{
    public class OperadorRepository : IOperadorRepository
    {
        private readonly AppDbContext _context;

        public OperadorRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Operador> GetOperadorById(int id)
        {
            return await _context.Operador
                            .FirstOrDefaultAsync(o => o.id == id);
        }

        public async Task<IEnumerable<Operador>> GetOperadores()
        {
            return await _context.Operador
                                .ToListAsync();
        }

        public async Task<IEnumerable<OperadorTela>> GetTelas(int id)
        {
            return await _context.OperadorTela
                            .Include(o => o.tela)
                            .Where(o => o.operador.id == id)
                            .ToListAsync();
        }

        public async Task<Operador> PostOperador(OperadorDTO operadorDto)
        {
            var operador = new Operador
            {
                nome = operadorDto.nome,
                senha = operadorDto.senha,
                admin = operadorDto.admin,
                inativo = operadorDto.inativo
            };

            var resultado = _context.Operador.AddAsync(operador);
            await _context.SaveChangesAsync();
            return resultado.Result.Entity;
        }

        public async Task<OperadorTela> PostOperadorTela(OperadorTelaDTO operadorTelaDTO)
        {
            var tela = await _context.Tela
                        .FirstOrDefaultAsync(t => t.Id ==  operadorTelaDTO.idTela);

            var telaOperador = new OperadorTela
            {
                operadorId = operadorTelaDTO.idOperador,
                telaId = operadorTelaDTO.idTela,
                ativo = operadorTelaDTO.ativo,
                novo = operadorTelaDTO.novo,
                editar = operadorTelaDTO.editar,
                excluir = operadorTelaDTO.excluir,
                tela = tela
            };

            var resultado = _context.OperadorTela.AddAsync(telaOperador);
            await _context.SaveChangesAsync();
            return resultado.Result.Entity;
        }
    }
}
