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

        public async Task<PedidoItem> DeleteItem(int id)
        {
            var item = await _context.PedidoItem.FindAsync(id);

            if (item is not null)
            {
                _context.Remove(item);
                await _context.SaveChangesAsync();
            }

            return item;
        }

        public async Task<Pedido> DeletePedido(int id)
        {
            var pedido = await _context.Pedido.FindAsync(id);

            if (pedido is not null)
            {
                var itens = await _context.PedidoItem.ToListAsync();

                if (itens is not null)
                {
                    foreach(var item in itens)
                    {
                        _context.Remove(item);
                    }
                }

                _context.Remove(pedido);
                await _context.SaveChangesAsync();
            }

            return pedido;
        }

        public async Task<IEnumerable<Pedido>> GetAllPedidos()
        {
            return await _context.Pedido
                .Include(p => p.formaPgto)
                .Include(p => p.cliente)
                .ToListAsync();
        }

        public async Task<IEnumerable<PedidoItem>> GetAllItens(int id)
        {
            return await _context.PedidoItem
                            .Include(i => i.produto)
                            .Where(i => i.pedidoId == id)
                            .ToListAsync();
        }

        public async Task<Pedido> GetPedidoById(int id)
        {
            return await _context.Pedido
                .Include(p => p.formaPgto)
                .Include(p => p.cliente)
                .FirstOrDefaultAsync(p => p.id == id);
        }

        public async Task<PedidoItem> PatchItem(int id, PedidoItemDTO itemDto)
        {
            var item = await _context.PedidoItem.FindAsync(id);

            if (item is not null)
            {
                item.total = itemDto.total;
                item.valor = itemDto.valor;
                item.produtoId = itemDto.produtoId;
                item.quantidade = itemDto.quantidade;
                item.desconto = itemDto.desconto;

                await _context.SaveChangesAsync();
            }

            return item;
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

        public async Task<PedidoItem> PostItem(PedidoItemDTO itemDto)
        {
            var item = new PedidoItem
            {
                desconto = itemDto.desconto,
                pedidoId = itemDto.pedidoId,
                produtoId = itemDto.produtoId,
                quantidade = itemDto.quantidade,
                total = itemDto.total,
                valor = itemDto.valor
            };

            var resultado = _context.PedidoItem.AddAsync(item);
            await _context.SaveChangesAsync();
            return resultado.Result.Entity;
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
