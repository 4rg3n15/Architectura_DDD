using System;
using System.Threading.Tasks;
using Arquitectura_DDD.Core.Entities;
using Arquitectura_DDD.Core.Interfaces;

namespace Arquitectura_DDD.Core.Services
{
    public class ServicioValidacionCredito : IServicioValidacionCredito
    {
        private readonly IClienteRepository _clienteRepository;
        private readonly IPedidoVentaRepository _pedidoRepository;

        public ServicioValidacionCredito(IClienteRepository clienteRepository, IPedidoVentaRepository pedidoRepository)
        {
            _clienteRepository = clienteRepository ?? throw new ArgumentNullException(nameof(clienteRepository));
            _pedidoRepository = pedidoRepository ?? throw new ArgumentNullException(nameof(pedidoRepository));
        }

        public async Task<bool> ValidarCapacidadPagoAsync(Guid clienteId, decimal montoPedido)
        {
            var cliente = await _clienteRepository.GetByIdAsync(clienteId);
            if (cliente == null)
                return false;

            return cliente.TieneCreditoDisponible(montoPedido);
        }

        public async Task<decimal> ConsultarHistorialCreditoAsync(Guid clienteId)
        {
            var cliente = await _clienteRepository.GetByIdAsync(clienteId);
            if (cliente == null)
                throw new InvalidOperationException("El cliente no existe");

            // Aquí se implementaría la lógica para consultar el historial de crédito
            // Por ejemplo, consultando pagos pendientes, historial de pagos, etc.
            return cliente.LimiteCredito;
        }

        public async Task<decimal> ConsultarLimiteCreditoAsync(Guid clienteId)
        {
            var cliente = await _clienteRepository.GetByIdAsync(clienteId);
            if (cliente == null)
                throw new InvalidOperationException("El cliente no existe");

            return cliente.LimiteCredito;
        }

        public async Task<decimal> ConsultarPagosPendientesAsync(Guid clienteId)
        {
            // Aquí se implementaría la lógica para consultar pagos pendientes
            // Por ejemplo, sumando todos los pedidos que no están pagados
            await Task.CompletedTask;
            return 0m; // Por ahora retornamos 0
        }

        public async Task<bool> ValidarPagosPendientesAsync(Guid clienteId)
        {
            var pagosPendientes = await ConsultarPagosPendientesAsync(clienteId);
            return pagosPendientes == 0;
        }

        public async Task RechazarPedidoPorLimiteCreditoAsync(Guid clienteId, decimal montoPedido)
        {
            var cliente = await _clienteRepository.GetByIdAsync(clienteId);
            if (cliente == null)
                throw new InvalidOperationException("El cliente no existe");

            // Aquí se implementaría la lógica para rechazar el pedido
            // Por ejemplo, registrar el rechazo, notificar al cliente, etc.
            await Task.CompletedTask;
        }
    }
}
