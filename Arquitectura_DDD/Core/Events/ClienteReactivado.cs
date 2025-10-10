using System;
using Arquitectura_DDD.Core.Events;

namespace Arquitectura_DDD.Core.Events
{
    public sealed class ClienteReactivado : DomainEvent
    {
        public Guid ClienteId { get; }

        public ClienteReactivado(Guid clienteId)
        {
            ClienteId = clienteId;
        }
    }
}
