using System;
using System.ComponentModel.DataAnnotations;

namespace Arquitectura_DDD.Application.DTOs
{
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
}
