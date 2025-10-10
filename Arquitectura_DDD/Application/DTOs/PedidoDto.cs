using System;
using System.Collections.Generic;

namespace Arquitectura_DDD.Application.DTOs
{
    public class PedidoDto
    {
        public Guid Id { get; set; }
        public string NumeroPedido { get; set; } = string.Empty;
        public Guid ClienteId { get; set; }
        public string ClienteNombre { get; set; } = string.Empty;
        public DateTime FechaCreacion { get; set; }
        public string Estado { get; set; } = string.Empty;
        public decimal MontoTotal { get; set; }
        public List<DetallePedidoDto> Detalles { get; set; } = new();
        public MetodoPagoDto? MetodoPago { get; set; }
    }
}
