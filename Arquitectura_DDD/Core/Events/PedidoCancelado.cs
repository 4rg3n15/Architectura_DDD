using System;
using Arquitectura_DDD.Core.Events;

namespace Arquitectura_DDD.Core.Events
{
    public class PedidoCancelado : DomainEvent
    {
        public Guid PedidoId { get; }
        public string Motivo { get; }
        public DateTime FechaCancelacion { get; }
        public decimal MontoReembolsar { get; }

        public PedidoCancelado(Guid pedidoId, string motivo, DateTime fechaCancelacion, decimal montoReembolsar)
        {
            PedidoId = pedidoId;
            Motivo = motivo;
            FechaCancelacion = fechaCancelacion;
            MontoReembolsar = montoReembolsar;
        }
    }
}
