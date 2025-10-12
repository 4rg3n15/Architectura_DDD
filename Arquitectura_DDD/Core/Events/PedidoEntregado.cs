using System;
using Arquitectura_DDD.Core.Events;

namespace Arquitectura_DDD.Core.Events
{
    public class PedidoEntregado : DomainEvent
    {
        public Guid PedidoId { get; }
        public DateTime FechaEntrega { get; }
        public string PersonaRecibe { get; }

        public PedidoEntregado(Guid pedidoId, DateTime fechaEntrega, string personaRecibe)
        {
            PedidoId = pedidoId;
            FechaEntrega = fechaEntrega;
            PersonaRecibe = personaRecibe;
        }
    }
}
