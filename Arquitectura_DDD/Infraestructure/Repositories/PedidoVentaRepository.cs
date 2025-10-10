using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Arquitectura_DDD.Core.Aggregates;
using Arquitectura_DDD.Core.Interfaces;
using Arquitectura_DDD.Infraestructure.Persistence;

namespace Arquitectura_DDD.Infraestructure.Repositories
{
    public class PedidoVentaRepository : IPedidoVentaRepository
    {
        private readonly VentasDbContext _context;

        public PedidoVentaRepository(VentasDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IUnitOfWork UnitOfWork => _context;

        public async Task<PedidoVenta> GetByIdAsync(Guid id)
        {
            return await _context.Pedidos
                .Include(p => p.Detalles)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<PedidoVenta>> GetAllAsync()
        {
            return await _context.Pedidos
                .Include(p => p.Detalles)
                .ToListAsync();
        }

        public async Task<PedidoVenta> GetByNumeroAsync(string numeroPedido)
        {
            return await _context.Pedidos
                .Include(p => p.Detalles)
                .FirstOrDefaultAsync(p => p.NumeroPedido == numeroPedido);
        }

        public async Task<IEnumerable<PedidoVenta>> GetByClienteIdAsync(Guid clienteId)
        {
            return await _context.Pedidos
                .Include(p => p.Detalles)
                .Where(p => p.ClienteId == clienteId)
                .ToListAsync();
        }

        public async Task AddAsync(PedidoVenta pedido)
        {
            await _context.Pedidos.AddAsync(pedido);
        }

        public async Task UpdateAsync(PedidoVenta pedido)
        {
            _context.Pedidos.Update(pedido);
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(Guid id)
        {
            var pedido = await GetByIdAsync(id);
            if (pedido != null)
            {
                _context.Pedidos.Remove(pedido);
            }
        }
    }
}
