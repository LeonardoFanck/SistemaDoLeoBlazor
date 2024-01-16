using Microsoft.EntityFrameworkCore;
using SistemaDoLeoBlazor.API.Context;
using SistemaDoLeoBlazor.API.Entities;
using SistemaDoLeoBlazor.MODELS.ProximoRegistroDTO;

namespace SistemaDoLeoBlazor.API.Repositories.ProximoRegistroRepository
{
    public class ProximoRegistroRepository : IProximoRegistroRepository
    {
        private readonly AppDbContext _context;

        public ProximoRegistroRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ProximoRegistro> GetRegistro()
        {
            return await _context.ProximoRegistro.FirstOrDefaultAsync();
        }

        public async Task<ProximoRegistro> PatchRegistro(ProximoRegistroDTO proximoRegistroDTO)
        {
            var registro = await _context.ProximoRegistro.FindAsync(proximoRegistroDTO.id);

            if (registro is not null)
            {
                registro.produto = proximoRegistroDTO.produto;
                registro.cliente = proximoRegistroDTO.cliente;
                registro.categoria = proximoRegistroDTO.categoria;
                registro.operador = proximoRegistroDTO.operador;
                registro.formaPgto = proximoRegistroDTO.formaPgto;
                registro.pedido = proximoRegistroDTO.pedido;

                await _context.SaveChangesAsync();
            }

            return registro;
        }
    }
}
