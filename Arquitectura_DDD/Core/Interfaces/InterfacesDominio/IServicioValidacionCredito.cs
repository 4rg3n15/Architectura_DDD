using System;
using System.Threading.Tasks;

namespace Arquitectura_DDD.Core.Interfaces
{
    public interface IServicioValidacionCredito
    {
        Task<bool> ValidarCapacidadPagoAsync(Guid clienteId, decimal montoPedido);
        Task<decimal> ConsultarHistorialCreditoAsync(Guid clienteId);
        Task<decimal> ConsultarLimiteCreditoAsync(Guid clienteId);
        Task<decimal> ConsultarPagosPendientesAsync(Guid clienteId);
        Task<bool> ValidarPagosPendientesAsync(Guid clienteId);
        Task RechazarPedidoPorLimiteCreditoAsync(Guid clienteId, decimal montoPedido);
    }
}
