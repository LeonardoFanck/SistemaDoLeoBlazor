using Microsoft.EntityFrameworkCore;
using SistemaDoLeoBlazor.API.Context;
using SistemaDoLeoBlazor.API.Entities;
using SistemaDoLeoBlazor.MODELS.FormaPgtoDTO;

namespace SistemaDoLeoBlazor.API.Repositories.FormaPgtoRepository
{
    public class FormaPgtoRepository : IFormaPgtoRepository
    {
        private readonly AppDbContext _context;

        public FormaPgtoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<FormaPgto> DeleteFormaPgto(int id)
        {
            var formaPgto = await _context.FormaPgto.FindAsync(id);

            if (formaPgto is not null)
            {
                _context.Remove(formaPgto);
                await _context.SaveChangesAsync();
            }

            return formaPgto;
        }

        public async Task<IEnumerable<FormaPgto>> GetAllFormaPgto()
        {
            return await _context.FormaPgto.ToListAsync();
        }

        public async Task<FormaPgto> GetFormaPgtoById(int id)
        {
            return await _context.FormaPgto.FindAsync(id);
        }

        public async Task<FormaPgto> PatchFormaPgto(int id, FormaPgtoDTO formaPgtoDTO)
        {
            var formaPgto = await _context.FormaPgto.FindAsync(id);

            if (formaPgto is not null)
            {
                formaPgto.nome = formaPgtoDTO.nome;
                formaPgto.inativo = formaPgtoDTO.inativo;

                await _context.SaveChangesAsync();
            }

            return formaPgto;
        }

        public async Task<FormaPgto> PostFormaPgto(FormaPgtoDTO formaPgtoDTO)
        {
            var formaPgto = new FormaPgto{
                nome = formaPgtoDTO.nome,
                inativo = formaPgtoDTO.inativo
            };

            var resultado = _context.FormaPgto.AddAsync(formaPgto);
            await _context.SaveChangesAsync();
            return resultado.Result.Entity;
        }
    }
}
