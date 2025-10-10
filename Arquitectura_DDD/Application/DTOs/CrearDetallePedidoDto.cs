using System;
using System.ComponentModel.DataAnnotations;

namespace Arquitectura_DDD.Application.DTOs
{
    public class CrearDetallePedidoDto
    {
        [Required]
        public Guid ProductoId { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 1)]
        public string NombreProducto { get; set; } = string.Empty;

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0")]
        public int Cantidad { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0")]
        public decimal PrecioUnitario { get; set; }
    }
}
