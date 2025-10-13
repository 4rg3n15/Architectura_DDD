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
        private readonly IClienteRepository _clienteRepository;

        public ServicioProcesamientoVentas(IPedidoVentaRepository pedidoRepository, IClienteRepository clienteRepository)
        {
            _pedidoRepository = pedidoRepository;
            _clienteRepository = clienteRepository;
        }

        public async Task ConfirmarVentaAsync(Guid pedidoId, MetodoPago metodoPago)
        {
            var pedido = await _pedidoRepository.GetByIdAsync(pedidoId);
            if (pedido == null)
                throw new ArgumentException($"Pedido {pedidoId} no encontrado");

            pedido.ConfirmarPago(metodoPago);

            await _pedidoRepository.UpdateAsync(pedido);
        }

        public async Task<string> GenerarFacturaParaPedido(Guid pedidoId, string nitCliente)
        {
            var pedido = await _pedidoRepository.GetByIdAsync(pedidoId);
            if (pedido == null)
                throw new ArgumentException($"Pedido {pedidoId} no encontrado");

            var numeroFactura = GenerarNumeroFactura();
            pedido.GenerarFactura(numeroFactura, nitCliente);

            await _pedidoRepository.UpdateAsync(pedido);
            return numeroFactura;
        }

        private string GenerarNumeroFactura()
        {
            return $"FE-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper()}";
        }
    }
}
