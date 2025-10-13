using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Arquitectura_DDD.Core.Aggregates;

namespace Arquitectura_DDD.Core.Interfaces
{
    public interface IPedidoVentaRepository : IRepository<PedidoVenta>
    {
        Task<IEnumerable<PedidoVenta>> GetByClienteIdAsync(Guid clienteId);
        Task<IEnumerable<PedidoVenta>> GetPedidosPendientesAsync();
        Task<PedidoVenta> GetByNumeroAsync(string numeroPedido);
        Task<bool> ExistsAsync(Guid id);
    }
}
