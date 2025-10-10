using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Arquitectura_DDD.Application.DTOs
{
    public sealed record CrearPedidoDto
    {
        [Required]
        public Guid ClienteId { get; init; }

        [Required]
        [MinLength(1)]
        public List<DetallePedidoDto> Detalles { get; init; } = new();

        [Required]
        public MetodoPagoDto MetodoPago { get; init; }
    }
}
