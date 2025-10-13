using System;
using Arquitectura_DDD.Core.Events;

namespace Arquitectura_DDD.Core.Events
{
    public class PedidoCancelado : DomainEvent
    {
        public Guid PedidoId { get; }
        public Guid ClienteId { get; }
        public string NumeroPedido { get; }
        public string Motivo { get; }

        public PedidoCancelado(Guid pedidoId, Guid clienteId, string numeroPedido, string motivo)
        {
            PedidoId = pedidoId;
            ClienteId = clienteId;
            NumeroPedido = numeroPedido;
            Motivo = motivo;
        }
    }
}
