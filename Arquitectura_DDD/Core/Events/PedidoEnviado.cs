using System;
using Arquitectura_DDD.Core.Events;

namespace Arquitectura_DDD.Core.Events
{
    public class PedidoEnviado : DomainEvent
    {
        public Guid PedidoId { get; }
        public string DireccionEntrega { get; }
        public string EmpresaMensajeria { get; }
        public string NumeroGuia { get; }
        public DateTime FechaEnvio { get; }

        public PedidoEnviado(Guid pedidoId, string direccionEntrega, string empresaMensajeria, string numeroGuia, DateTime fechaEnvio)
        {
            PedidoId = pedidoId;
            DireccionEntrega = direccionEntrega;
            EmpresaMensajeria = empresaMensajeria;
            NumeroGuia = numeroGuia;
            FechaEnvio = fechaEnvio;
        }
    }
}
