using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Arquitectura_DDD.Application.DTOs
{
    public class CrearPedidoRequestDto
    {
        [Required]
        public Guid ClienteId { get; set; }

        [Required]
        [MinLength(1, ErrorMessage = "Debe incluir al menos un detalle")]
        public List<CrearDetallePedidoDto> Detalles { get; set; } = new();
    }
}
