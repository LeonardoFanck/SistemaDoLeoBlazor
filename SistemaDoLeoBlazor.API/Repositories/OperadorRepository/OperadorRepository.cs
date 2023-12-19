using Microsoft.AspNetCore.Http.HttpResults;
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
                            .Where(o => o.operadorId == id)
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

        public async Task PostOperadorTela(OperadorDTO operadorDTO)
        {
            //var tela = await _context.Tela
            //            .FirstOrDefaultAsync(t => t.Id == operadorTelaDTO.idTela);


            var teste =  await _context.Tela
                                .Where(t => !_context.OperadorTela.Any(ot => ot.telaId == t.Id && ot.operadorId == operadorDTO.id))
                                .ToListAsync();
                  
            if (teste.Count == 0)
            {
                throw new Exception("Operador Já possuí todas as telas disponiveis");
            }

            foreach (var tela in teste)
            {
                var novoOperadorTela = new OperadorTela
                {
                    operadorId = operadorDTO.id,
                    telaId = tela.Id,
                    ativo = true,
                    novo = true,
                    editar = true,
                    excluir = true,
                };

                await _context.OperadorTela.AddAsync(novoOperadorTela);
            }
            
            await _context.SaveChangesAsync();
            //return resultado.Result.Entity;
        }

        public async Task<Operador> DeleteOperador(int id)
        {
            var operador = await _context.Operador.FindAsync(id);

            if (operador is not null)
            {
                _context.Operador.Remove(operador);
                await _context.SaveChangesAsync();
            }

            return operador;
        }

        public async Task<Operador> PatchOperador(int id, OperadorDTO operadorDto)
        {
            var operador = await _context.Operador.FindAsync(id);

            if (operador is not null){
                operador.nome = operadorDto.nome;
                operador.senha = operadorDto.senha;
                operador.admin = operadorDto.admin;
                operador.inativo = operadorDto.inativo;

                await _context.SaveChangesAsync();
                return operador;
            }

            return operador;
        }

        public async Task<OperadorTela> PatchOperadorTela(int id, OperadorTelaDTO operadorTelaDto)
        {
            var tela = await _context.OperadorTela.FindAsync(id);

            tela.tela = await _context.Tela.FindAsync(operadorTelaDto.idTela);

            if (tela.tela is null)
            {
                return null;
            }

            if (tela is not null)
            {
                tela.ativo = operadorTelaDto.ativo;
                tela.novo = operadorTelaDto.novo;
                tela.editar = operadorTelaDto.editar;
                tela.excluir = operadorTelaDto.excluir;

                await _context.SaveChangesAsync();

                return tela;
            }

            return tela;
        }
    }
}
