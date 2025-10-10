using System;
using Arquitectura_DDD.Core.Events;

namespace Arquitectura_DDD.Core.Events
{
    public sealed class ClienteCreado : DomainEvent
    {
        public Guid ClienteId { get; }
        public string Nombre { get; }
        public string Email { get; }

        public ClienteCreado(Guid clienteId, string nombre, string email)
        {
            ClienteId = clienteId;
            Nombre = nombre;
            Email = email;
        }
    }
}
