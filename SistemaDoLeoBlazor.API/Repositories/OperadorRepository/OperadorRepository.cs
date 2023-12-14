using Microsoft.EntityFrameworkCore;
using SistemaDoLeoBlazor.API.Context;
using SistemaDoLeoBlazor.API.Entities;

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
                            .Include(o => o.operadorPermissoesTela)
                            .ToListAsync();
        }
    }
}
