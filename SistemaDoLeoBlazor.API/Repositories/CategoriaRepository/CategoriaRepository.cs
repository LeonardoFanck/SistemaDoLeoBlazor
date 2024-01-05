using Microsoft.EntityFrameworkCore;
using SistemaDoLeoBlazor.API.Context;
using SistemaDoLeoBlazor.API.Entities;
using SistemaDoLeoBlazor.MODELS.CategoriaDTO;

namespace SistemaDoLeoBlazor.API.Repositories.CategoriaRepository
{
    public class CategoriaRepository : ICategoriaRepository
    {
        private readonly AppDbContext _context;

        public CategoriaRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Categoria> DeleteCategoria(int id)
        {
            var categoria = await _context.Categoria.FindAsync(id);

            if (categoria is not null)
            {
                _context.Categoria.Remove(categoria);
                await _context.SaveChangesAsync();
            }

            return categoria;
        }

        public async Task<IEnumerable<Categoria>> GetAllCategorias()
        {
            return await _context.Categoria
                                .ToListAsync();
        }

        public async Task<Categoria> GetCategoriaById(int id)
        {
            return await _context.Categoria.FindAsync(id);
        }

        public async Task<Categoria> PatchCategoria(int id, CategoriaDTO categoriaDTO)
        {
            var categoria = await _context.Categoria.FindAsync(id);
            
            if (categoria is not null)
            {
                categoria.nome = categoriaDTO.nome;
                categoria.inativo = categoriaDTO.inativo;

                await _context.SaveChangesAsync();
                return categoria;
            }

            return categoria;
        }

        public async Task<Categoria> PostCategoria(CategoriaDTO categoriaDTO)
        {
            var categoria = new Categoria
            {
                nome = categoriaDTO.nome,
                inativo = categoriaDTO.inativo
            };

            var resultado = _context.Categoria.AddAsync(categoria);
            await _context.SaveChangesAsync();
            return resultado.Result.Entity;
        }
    }
}
