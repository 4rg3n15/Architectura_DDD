using System;
using System.Threading.Tasks;
using Arquitectura_DDD.Core.Aggregates;
using Arquitectura_DDD.Core.ValueObjects;
using Arquitectura_DDD.Core.Interfaces;

namespace Arquitectura_DDD.Core.Services
{
    public class ServicioProcesamientoVentas
    {
        private readonly IPedidoVentaRepository _pedidoRepository;

        public ServicioProcesamientoVentas(IPedidoVentaRepository pedidoRepository)
        {
            _pedidoRepository = pedidoRepository ?? throw new ArgumentNullException(nameof(pedidoRepository));
        }

        public async Task ConfirmarVentaAsync(Guid pedidoId, MetodoPago metodoPago)
        {
            var pedido = await _pedidoRepository.GetByIdAsync(pedidoId);
            if (pedido == null)
                throw new InvalidOperationException("El pedido no existe");

            pedido.ConfirmarPago(metodoPago);
            await _pedidoRepository.UpdateAsync(pedido);
        }

        public async Task RegistrarPagoAsync(Guid pedidoId, MetodoPago metodoPago)
        {
            await ConfirmarVentaAsync(pedidoId, metodoPago);
        }

        public async Task EmitirFacturaAsync(Guid pedidoId, string numeroFactura, string nitCliente)
        {
            var pedido = await _pedidoRepository.GetByIdAsync(pedidoId);
            if (pedido == null)
                throw new InvalidOperationException("El pedido no existe");

            pedido.GenerarFactura(numeroFactura, nitCliente);
            await _pedidoRepository.UpdateAsync(pedido);
        }

        public async Task DispararEventosDominioAsync(PedidoVenta pedido)
        {
            // Los eventos se disparan automáticamente desde el agregado
            // Aquí podríamos implementar lógica adicional si fuera necesario
            await Task.CompletedTask;
        }
    }
}
