using Microsoft.EntityFrameworkCore;
using SistemaDoLeoBlazor.API.Context;
using SistemaDoLeoBlazor.API.Entities;
using SistemaDoLeoBlazor.MODELS.PedidoDTO;

namespace SistemaDoLeoBlazor.API.Repositories.PedidoRepository
{
    public class PedidoRepository : IPedidoRepository
    {
        private readonly AppDbContext _context;

        public PedidoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Pedido> DeletePedido(int id)
        {
            var pedido = await _context.Pedido.FindAsync(id);

            if (pedido is not null)
            {
                _context.Remove(pedido);
                await _context.SaveChangesAsync();
            }

            return pedido;
        }

        public async Task<IEnumerable<Pedido>> GetAllPedidos()
        {
            return await _context.Pedido.ToListAsync();
        }

        public async Task<Pedido> GetPedidoById(int id)
        {
            return await _context.Pedido.FindAsync(id);
        }

        public async Task<Pedido> PatchPedido(int id, PedidoDTO pedidoDTO)
        {
            var pedido = await _context.Pedido.FindAsync(id);

            if (pedido is not null)
            {
                pedido.valor = pedidoDTO.valor;
                pedido.total = pedidoDTO.total;
                pedido.clienteId = pedidoDTO.clienteId;
                pedido.formaPgtoId = pedidoDTO.formaPgtoId;
                pedido.data = pedidoDTO.data;
                pedido.desconto = pedidoDTO.desconto;

                await _context.SaveChangesAsync();
            }

            return pedido;
        }

        public async Task<Pedido> PostPedido(PedidoDTO pedidoDTO)
        {
            var pedido = new Pedido
            {
                clienteId = pedidoDTO.clienteId,
                data = pedidoDTO.data,
                desconto = pedidoDTO.desconto,
                formaPgtoId = pedidoDTO.formaPgtoId,
                total = pedidoDTO.total,
                valor = pedidoDTO.valor
            };

            var resultado = _context.Pedido.AddAsync(pedido);
            await _context.SaveChangesAsync();
            return resultado.Result.Entity;
        }
    }
}
