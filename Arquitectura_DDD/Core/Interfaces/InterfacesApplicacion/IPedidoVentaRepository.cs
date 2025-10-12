using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Arquitectura_DDD.Core.Aggregates;

namespace Arquitectura_DDD.Core.Interfaces
{
    public interface IPedidoVentaRepository
    {
        Task<PedidoVenta> GetByIdAsync(Guid id);
        Task<IEnumerable<PedidoVenta>> GetByClienteIdAsync(Guid clienteId);
        Task<IEnumerable<PedidoVenta>> GetPedidosPendientesAsync();
        Task AddAsync(PedidoVenta pedido);
        Task UpdateAsync(PedidoVenta pedido);
        Task DeleteAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
    }
}
