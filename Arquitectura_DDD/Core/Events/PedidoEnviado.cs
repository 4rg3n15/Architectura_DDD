using System;
using Arquitectura_DDD.Core.Events;

namespace Arquitectura_DDD.Core.Events
{
    public class PedidoEnviado : DomainEvent
    {
        public Guid PedidoId { get; }
        public Guid ClienteId { get; }
        public string NumeroPedido { get; }

        public PedidoEnviado(Guid pedidoId, Guid clienteId, string numeroPedido)
        {
            PedidoId = pedidoId;
            ClienteId = clienteId;
            NumeroPedido = numeroPedido;
        }
    }
}
