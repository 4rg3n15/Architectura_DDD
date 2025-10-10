using System;
using Arquitectura_DDD.Core.Events;

namespace Arquitectura_DDD.Core.Events
{
    public sealed class PedidoFacturado : DomainEvent
    {
        public Guid PedidoId { get; }
        public Guid ClienteId { get; }
        public string NumeroFactura { get; }
        public decimal ValorTotal { get; }

        public PedidoFacturado(Guid pedidoId, Guid clienteId, string numeroFactura, decimal valorTotal)
        {
            PedidoId = pedidoId;
            ClienteId = clienteId;
            NumeroFactura = numeroFactura;
            ValorTotal = valorTotal;
        }
    }
}
