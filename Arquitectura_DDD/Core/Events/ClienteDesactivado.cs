using System;
using Arquitectura_DDD.Core.Events;

namespace Arquitectura_DDD.Core.Events
{
    public sealed class ClienteDesactivado : DomainEvent
    {
        public Guid ClienteId { get; }

        public ClienteDesactivado(Guid clienteId)
        {
            ClienteId = clienteId;
        }
    }
}
