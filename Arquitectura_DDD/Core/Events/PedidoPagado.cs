using System;
using Arquitectura_DDD.Core.Events;

namespace Arquitectura_DDD.Core.Events
{
    public class PedidoPagado : DomainEvent
    {
        public Guid PedidoId { get; }
        public Guid ClienteId { get; }
        public string MetodoPago { get; }
        public decimal MontoPagado { get; }

        public PedidoPagado(Guid pedidoId, Guid clienteId, decimal montoPagado, string metodoPago)
        {
            PedidoId = pedidoId;
            ClienteId = clienteId;
            MontoPagado = montoPagado;
            MetodoPago = metodoPago;
        }
    }
}
