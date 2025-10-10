using System;
using Arquitectura_DDD.Core.Events;

namespace Arquitectura_DDD.Core.Events
{
    public sealed class PedidoPagado : DomainEvent
    {
        public Guid PedidoId { get; }
        public Guid ClienteId { get; }
        public decimal MontoTotal { get; }
        public string MetodoPago { get; }

        public PedidoPagado(Guid pedidoId, Guid clienteId, decimal montoTotal, string metodoPago)
        {
            PedidoId = pedidoId;
            ClienteId = clienteId;
            MontoTotal = montoTotal;
            MetodoPago = metodoPago;
        }
    }
}
