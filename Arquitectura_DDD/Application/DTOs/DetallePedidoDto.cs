using System.ComponentModel.DataAnnotations;

namespace Arquitectura_DDD.Application.DTOs
{
    public sealed record DetallePedidoDto
    {
        [Required]
        public Guid ProductoId { get; init; }

        [Required]
        [StringLength(100)]
        public string NombreProducto { get; init; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Cantidad { get; init; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal PrecioUnitario { get; init; }
    }
}
