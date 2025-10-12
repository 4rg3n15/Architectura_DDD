using System;
using System.Collections.Generic;
using System.Linq;
using Arquitectura_DDD.Core.Common;
using Arquitectura_DDD.Core.ValueObjects;
using Arquitectura_DDD.Core.Events;

namespace Arquitectura_DDD.Core.Aggregates
{
    public class PedidoVenta : AggregateRoot
    {
        public Guid Id { get; private set; }
        public Guid ClienteId { get; private set; }
        public EstadoPedido Estado { get; private set; }
        public MontoTotal MontoTotal { get; private set; }
        public MetodoPago MetodoPago { get; private set; }
        public DireccionEntrega DireccionEntrega { get; private set; }
        public DatosFactura DatosFactura { get; private set; }
        public DateTime FechaCreacion { get; private set; }
        public DateTime? FechaUltimaActualizacion { get; private set; }

        private readonly List<DetallePedido> _detalles;
        public IReadOnlyCollection<DetallePedido> Detalles => _detalles.AsReadOnly();

        private readonly List<DomainEvent> _domainEvents;
        public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        // Constructor privado para EF
        private PedidoVenta()
        {
            _detalles = new List<DetallePedido>();
            _domainEvents = new List<DomainEvent>();
        }

        public PedidoVenta(Guid id, Guid clienteId, DireccionEntrega direccionEntrega)
        {
            Id = id;
            ClienteId = clienteId;
            DireccionEntrega = direccionEntrega ?? throw new ArgumentNullException(nameof(direccionEntrega));
            Estado = EstadoPedido.Create(EstadoPedido.Pendiente);
            MontoTotal = MontoTotal.Create(0, 0);
            _detalles = new List<DetallePedido>();
            _domainEvents = new List<DomainEvent>();
            FechaCreacion = DateTime.UtcNow;

            AddDomainEvent(new PedidoCreado(Id, ClienteId, 0, FechaCreacion, _detalles.ToList()));
        }

        // Comportamientos ricos - Raíz es único punto de acceso
        public void AgregarDetalle(DetallePedido detalle)
        {
            if (Estado.EsFinal)
                throw new InvalidOperationException("No se puede modificar un pedido finalizado");

            var detalleExistente = _detalles.FirstOrDefault(d => d.ProductoId == detalle.ProductoId);
            if (detalleExistente != null)
                throw new InvalidOperationException($"El producto {detalle.ProductoId} ya existe en el pedido");

            _detalles.Add(detalle);
            RecalcularMontoTotal();
            ActualizarFecha();
        }

        public void ActualizarCantidadDetalle(string productoId, int nuevaCantidad)
        {
            if (Estado.EsFinal)
                throw new InvalidOperationException("No se puede modificar un pedido finalizado");

            var detalle = _detalles.FirstOrDefault(d => d.ProductoId == productoId);
            if (detalle == null)
                throw new ArgumentException($"Producto {productoId} no encontrado en el pedido");

            var detalleActualizado = detalle.ActualizarCantidad(nuevaCantidad);
            _detalles.Remove(detalle);
            _detalles.Add(detalleActualizado);
            RecalcularMontoTotal();
            ActualizarFecha();
        }

        public void RemoverDetalle(string productoId)
        {
            if (Estado.EsFinal)
                throw new InvalidOperationException("No se puede modificar un pedido finalizado");

            var detalle = _detalles.FirstOrDefault(d => d.ProductoId == productoId);
            if (detalle == null)
                throw new ArgumentException($"Producto {productoId} no encontrado en el pedido");

            _detalles.Remove(detalle);
            RecalcularMontoTotal();
            ActualizarFecha();
        }

        public void AplicarMetodoPago(MetodoPago metodoPago)
        {
            MetodoPago = metodoPago ?? throw new ArgumentNullException(nameof(metodoPago));
            ActualizarFecha();
        }

        public void MarcarComoPagado()
        {
            if (Estado.CodigoEstado != EstadoPedido.Pendiente)
                throw new InvalidOperationException("Solo pedidos pendientes pueden ser pagados");

            if (MetodoPago == null)
                throw new InvalidOperationException("Debe asignar un método de pago antes de pagar");

            Estado = Estado.TransicionarA(EstadoPedido.Pagado);

            AddDomainEvent(new PedidoPagado(Id, MetodoPago.Tipo, MontoTotal.Total, DateTime.UtcNow));
            ActualizarFecha();
        }

        public void GenerarFactura(string numeroFactura, string nitCliente)
        {
            if (Estado.CodigoEstado != EstadoPedido.Pagado)
                throw new InvalidOperationException("Solo pedidos pagados pueden ser facturados");

            DatosFactura = DatosFactura.Create(numeroFactura, nitCliente, MontoTotal.Total);

            AddDomainEvent(new PedidoFacturado(Id, numeroFactura, DateTime.UtcNow, MontoTotal.Total));
            ActualizarFecha();
        }

        public void MarcarComoEnviado(string empresaMensajeria, string numeroGuia)
        {
            if (Estado.CodigoEstado != EstadoPedido.Pagado)
                throw new InvalidOperationException("Solo pedidos pagados pueden ser enviados");

            Estado = Estado.TransicionarA(EstadoPedido.Enviado);

            AddDomainEvent(new PedidoEnviado(Id, DireccionEntrega.ToString(), empresaMensajeria, numeroGuia, DateTime.UtcNow));
            ActualizarFecha();
        }

        public void MarcarComoEntregado(string personaRecibe)
        {
            if (Estado.CodigoEstado != EstadoPedido.Enviado)
                throw new InvalidOperationException("Solo pedidos enviados pueden ser entregados");

            Estado = Estado.TransicionarA(EstadoPedido.Entregado);

            AddDomainEvent(new PedidoEntregado(Id, DateTime.UtcNow, personaRecibe));
            ActualizarFecha();
        }

        public void Cancelar(string motivo)
        {
            if (!Estado.PuedeCancelar)
                throw new InvalidOperationException("No se puede cancelar el pedido en su estado actual");

            Estado = Estado.TransicionarA(EstadoPedido.Cancelado);

            AddDomainEvent(new PedidoCancelado(Id, motivo, DateTime.UtcNow, MontoTotal.Total));
            ActualizarFecha();
        }

        // Métodos privados para mantener consistencia
        private void RecalcularMontoTotal()
        {
            var subtotal = _detalles.Sum(d => d.Subtotal);
            // Suponiendo un 19% de IVA
            MontoTotal = MontoTotal.Create(subtotal, 19, 0);
        }

        private void ActualizarFecha()
        {
            FechaUltimaActualizacion = DateTime.UtcNow;
        }

        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }

        protected void AddDomainEvent(DomainEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }
    }
}