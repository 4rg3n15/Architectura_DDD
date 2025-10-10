using System.ComponentModel.DataAnnotations;

namespace Arquitectura_DDD.Application.DTOs
{
    public sealed record CrearPedidoDto
    {
        [Required]
        public Guid ClienteId { get; init; }

        [Required]
        [MinLength(1)]
        public List<DetallePedidoRequest> Detalles { get; init; } = new();

        [Required]
        public MetodoPagoRequest MetodoPago { get; init; }
    }
}
