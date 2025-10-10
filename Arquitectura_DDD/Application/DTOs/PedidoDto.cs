using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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

    public class DetallePedidoDto
    {
        public Guid ProductoId { get; set; }
        public string NombreProducto { get; set; } = string.Empty;
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal { get; set; }
    }

    public class MetodoPagoDto
    {
        public string Tipo { get; set; } = string.Empty;
        public string Proveedor { get; set; } = string.Empty;
        public string NumeroReferencia { get; set; } = string.Empty;
    }

    public class CrearPedidoRequestDto
    {
        [Required]
        public Guid ClienteId { get; set; }

        [Required]
        [MinLength(1, ErrorMessage = "Debe incluir al menos un detalle")]
        public List<CrearDetallePedidoDto> Detalles { get; set; } = new();
    }

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

    public class ConfirmarPagoRequestDto
    {
        [Required]
        public Guid PedidoId { get; set; }

        [Required]
        public string TipoPago { get; set; } = string.Empty;

        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string Proveedor { get; set; } = string.Empty;

        [Required]
        [StringLength(50, MinimumLength = 1)]
        public string NumeroReferencia { get; set; } = string.Empty;
    }

    public class CancelarPedidoRequestDto
    {
        [Required]
        public Guid PedidoId { get; set; }

        [Required]
        [StringLength(500, MinimumLength = 10, ErrorMessage = "El motivo debe tener entre 10 y 500 caracteres")]
        public string Motivo { get; set; } = string.Empty;
    }
}
