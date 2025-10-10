using System;
using System.Collections.Generic;
using System.Linq;
using Arquitectura_DDD.Core.Common;
using Arquitectura_DDD.Core.ValueObjects;
using Arquitectura_DDD.Core.Events;

namespace Arquitectura_DDD.Core.Aggregates
{
    public class PedidoVenta : Entity, IAggregateRoot
    {
        public string NumeroPedido { get; private set; } = string.Empty;
        public Guid ClienteId { get; private set; }
        public DateTime FechaCreacion { get; private set; }
        public EstadoPedido Estado { get; private set; } = null!;
        public MetodoPago? MetodoPago { get; private set; }
        public MontoTotal MontoTotal { get; private set; } = null!;

        private readonly List<DetallePedido> _detalles = new();
        public IReadOnlyCollection<DetallePedido> Detalles => _detalles.AsReadOnly();

        // Constructor privado para EF
        private PedidoVenta() { }

        public PedidoVenta(Guid clienteId, string numeroPedido)
        {
            if (clienteId == Guid.Empty)
                throw new ArgumentException("El ID del cliente no puede estar vacío", nameof(clienteId));
            if (string.IsNullOrWhiteSpace(numeroPedido))
                throw new ArgumentException("El número de pedido no puede estar vacío", nameof(numeroPedido));

            Id = Guid.NewGuid();
            ClienteId = clienteId;
            NumeroPedido = numeroPedido.Trim();
            FechaCreacion = DateTime.UtcNow;
            Estado = EstadoPedido.Pendiente();

            AddDomainEvent(new PedidoCreado(Id, ClienteId, NumeroPedido));
        }

        // Comportamientos ricos del agregado
        public void AgregarDetalle(Guid productoId, string nombreProducto, int cantidad, decimal precioUnitario)
        {
            if (Estado.Codigo != EstadoPedido.CodigoEstado.Pendiente)
                throw new InvalidOperationException("Solo se pueden agregar detalles a pedidos pendientes");

            var detalleExistente = _detalles.FirstOrDefault(d => d.ProductoId == productoId);
            if (detalleExistente != null)
            {
                // Remove existing detail and add new one with updated quantity
                _detalles.Remove(detalleExistente);
                var nuevoDetalle = new DetallePedido(productoId, nombreProducto, 
                    detalleExistente.Cantidad + cantidad, precioUnitario);
                _detalles.Add(nuevoDetalle);
            }
            else
            {
                var nuevoDetalle = new DetallePedido(productoId, nombreProducto, cantidad, precioUnitario);
                _detalles.Add(nuevoDetalle);
            }

            RecalcularMontoTotal();
        }

        public void RemoverDetalle(Guid productoId)
        {
            if (Estado.Codigo != EstadoPedido.CodigoEstado.Pendiente)
                throw new InvalidOperationException("Solo se pueden remover detalles de pedidos pendientes");

            var detalle = _detalles.FirstOrDefault(d => d.ProductoId == productoId);
            if (detalle != null)
            {
                _detalles.Remove(detalle);
                RecalcularMontoTotal();
            }
        }

        public void ConfirmarPago(MetodoPago metodoPago)
        {
            if (Estado.Codigo != EstadoPedido.CodigoEstado.Pendiente)
                throw new InvalidOperationException("Solo se pueden confirmar pagos de pedidos pendientes");

            if (_detalles.Count == 0)
                throw new InvalidOperationException("No se puede confirmar pago de un pedido sin detalles");

            MetodoPago = metodoPago ?? throw new ArgumentNullException(nameof(metodoPago));
            Estado = EstadoPedido.Pagado();

            AddDomainEvent(new PedidoPagado(Id, ClienteId, MontoTotal.Total, metodoPago.Tipo.ToString()));
        }

        public void MarcarComoEnviado()
        {
            if (Estado.Codigo != EstadoPedido.CodigoEstado.Pagado)
                throw new InvalidOperationException("Solo se pueden enviar pedidos pagados");

            Estado = EstadoPedido.Enviado();
            AddDomainEvent(new PedidoEnviado(Id, ClienteId, NumeroPedido));
        }

        public void MarcarComoEntregado()
        {
            if (Estado.Codigo != EstadoPedido.CodigoEstado.Enviado)
                throw new InvalidOperationException("Solo se pueden entregar pedidos enviados");

            Estado = EstadoPedido.Entregado();
            AddDomainEvent(new PedidoEntregado(Id, ClienteId, NumeroPedido));
        }

        public void Cancelar(string motivo)
        {
            if (Estado.Codigo == EstadoPedido.CodigoEstado.Entregado)
                throw new InvalidOperationException("No se puede cancelar un pedido ya entregado");

            if (string.IsNullOrWhiteSpace(motivo))
                throw new ArgumentException("El motivo de cancelación no puede estar vacío", nameof(motivo));

            Estado = EstadoPedido.Cancelado();
            AddDomainEvent(new PedidoCancelado(Id, ClienteId, NumeroPedido, motivo));
        }

        public void GenerarFactura(string numeroFactura, string nitCliente)
        {
            if (Estado.Codigo != EstadoPedido.CodigoEstado.Pagado)
                throw new InvalidOperationException("Solo se pueden generar facturas para pedidos pagados");

            if (string.IsNullOrWhiteSpace(numeroFactura))
                throw new ArgumentException("El número de factura no puede estar vacío", nameof(numeroFactura));

            AddDomainEvent(new PedidoFacturado(Id, ClienteId, numeroFactura, MontoTotal.Total));
        }

        private void RecalcularMontoTotal()
        {
            var subtotal = _detalles.Sum(d => d.Subtotal);
            var impuestos = subtotal * 0.19m; // 19% IVA
            var descuentos = 0m; // Podría implementarse lógica de descuentos

            MontoTotal = new MontoTotal(subtotal, impuestos, descuentos);
        }
    }
}