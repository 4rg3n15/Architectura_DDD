using System;
using Arquitectura_DDD.Core.Events;

namespace Arquitectura_DDD.Core.Events
{
    public class PedidoCreado : DomainEvent
    {
        public Guid PedidoId { get; }
        public Guid ClienteId { get; }
        public decimal Total { get; }
        public DateTime FechaCreacion { get; }
        public List<DetallePedido> Detalles { get; }

        public PedidoCreado(Guid pedidoId, Guid clienteId, decimal total, DateTime fechaCreacion, List<DetallePedido> detalles)
        {
            PedidoId = pedidoId;
            ClienteId = clienteId;
            Total = total;
            FechaCreacion = fechaCreacion;
            Detalles = detalles;
        }
    }
}