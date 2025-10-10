using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Arquitectura_DDD.Core.Aggregates;

namespace Arquitectura_DDD.Core.Interfaces
{
    public interface IPedidoVentaRepository : IRepository<PedidoVenta>
    {
        Task<PedidoVenta> GetByNumeroAsync(string numeroPedido);
        Task<IEnumerable<PedidoVenta>> GetByClienteIdAsync(Guid clienteId);
    }
}
