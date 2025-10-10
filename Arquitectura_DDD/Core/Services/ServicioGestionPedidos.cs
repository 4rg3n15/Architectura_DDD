using System;
using System.Threading.Tasks;
using Arquitectura_DDD.Core.Aggregates;
using Arquitectura_DDD.Core.Entities;
using Arquitectura_DDD.Core.ValueObjects;
using Arquitectura_DDD.Core.Interfaces;

namespace Arquitectura_DDD.Core.Services
{
    public class ServicioGestionPedidos
    {
        private readonly IClienteRepository _clienteRepository;
        private readonly IPedidoVentaRepository _pedidoRepository;

        public ServicioGestionPedidos(IClienteRepository clienteRepository, IPedidoVentaRepository pedidoRepository)
        {
            _clienteRepository = clienteRepository ?? throw new ArgumentNullException(nameof(clienteRepository));
            _pedidoRepository = pedidoRepository ?? throw new ArgumentNullException(nameof(pedidoRepository));
        }

        public async Task<PedidoVenta> CrearPedidoAsync(Guid clienteId, string numeroPedido)
        {
            var cliente = await _clienteRepository.GetByIdAsync(clienteId);
            if (cliente == null)
                throw new InvalidOperationException("El cliente no existe");

            if (!cliente.Activo)
                throw new InvalidOperationException("No se puede crear un pedido para un cliente inactivo");

            var pedido = new PedidoVenta(clienteId, numeroPedido);
            
            await _pedidoRepository.AddAsync(pedido);
            
            return pedido;
        }

        public async Task ValidarDisponibilidadProductosAsync(Guid productoId, int cantidad)
        {
            // Aquí se implementaría la lógica para validar disponibilidad
            // Por ejemplo, consultando un servicio de inventario
            // Por ahora, asumimos que todos los productos están disponibles
            await Task.CompletedTask;
        }

        public async Task ActualizarEstadoPedidoAsync(Guid pedidoId, EstadoPedido nuevoEstado)
        {
            var pedido = await _pedidoRepository.GetByIdAsync(pedidoId);
            if (pedido == null)
                throw new InvalidOperationException("El pedido no existe");

            // La lógica de cambio de estado está en el agregado
            switch (nuevoEstado.Codigo)
            {
                case EstadoPedido.CodigoEstado.Pagado:
                    // El pago debe confirmarse con método de pago
                    throw new InvalidOperationException("Use ConfirmarPago para cambiar a estado Pagado");
                case EstadoPedido.CodigoEstado.Enviado:
                    pedido.MarcarComoEnviado();
                    break;
                case EstadoPedido.CodigoEstado.Entregado:
                    pedido.MarcarComoEntregado();
                    break;
                case EstadoPedido.CodigoEstado.Cancelado:
                    throw new InvalidOperationException("Use Cancelar para cambiar a estado Cancelado");
            }

            await _pedidoRepository.UpdateAsync(pedido);
        }

        public async Task<MontoTotal> CalcularTotalConDescuentosYImpuestosAsync(decimal subtotal)
        {
            var impuestos = subtotal * 0.19m; // 19% IVA
            var descuentos = 0m; // Aquí se implementaría la lógica de descuentos
            
            return new MontoTotal(subtotal, impuestos, descuentos);
        }
    }
}
