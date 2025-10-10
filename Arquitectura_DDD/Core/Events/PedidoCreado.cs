using System;
using Arquitectura_DDD.Core.Events;

namespace Arquitectura_DDD.Core.Events
{
    public sealed class PedidoCreado : DomainEvent
    {
        public Guid PedidoId { get; }
        public Guid ClienteId { get; }
        public string NumeroPedido { get; }

        public PedidoCreado(Guid pedidoId, Guid clienteId, string numeroPedido)
        {
            PedidoId = pedidoId;
            ClienteId = clienteId;
            NumeroPedido = numeroPedido;
        }
    }
}