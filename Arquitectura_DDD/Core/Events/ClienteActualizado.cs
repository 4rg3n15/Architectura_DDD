using System;
using Arquitectura_DDD.Core.Events;

namespace Arquitectura_DDD.Core.Events
{
    public sealed class ClienteActualizado : DomainEvent
    {
        public Guid ClienteId { get; }

        public ClienteActualizado(Guid clienteId)
        {
            ClienteId = clienteId;
        }
    }
}
