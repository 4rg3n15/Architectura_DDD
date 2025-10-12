using System;
using Arquitectura_DDD.Core.Events;

namespace Arquitectura_DDD.Core.Events
{
    public class PedidoPagado : DomainEvent
    {
        public Guid PedidoId { get; }
        public string MetodoPago { get; }
        public decimal MontoPagado { get; }
        public DateTime FechaPago { get; }

        public PedidoPagado(Guid pedidoId, string metodoPago, decimal montoPagado, DateTime fechaPago)
        {
            PedidoId = pedidoId;
            MetodoPago = metodoPago;
            MontoPagado = montoPagado;
            FechaPago = fechaPago;
        }
    }
}
