using System;
using Arquitectura_DDD.Core.Events;

namespace Arquitectura_DDD.Core.Events
{
    public sealed class ReclamoRegistrado : DomainEvent
    {
        public Guid ReclamoId { get; }
        public Guid PedidoId { get; }
        public Guid ClienteId { get; }
        public string Descripcion { get; }
        public DateTime FechaRegistro { get; }

        public ReclamoRegistrado(Guid reclamoId, Guid pedidoId, Guid clienteId, string descripcion)
        {
            ReclamoId = reclamoId;
            PedidoId = pedidoId;
            ClienteId = clienteId;
            Descripcion = descripcion;
            FechaRegistro = DateTime.UtcNow;
        }
    }
}
