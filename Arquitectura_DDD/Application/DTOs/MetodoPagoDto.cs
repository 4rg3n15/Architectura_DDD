using System.ComponentModel.DataAnnotations;

namespace Arquitectura_DDD.Application.DTOs
{
    public sealed record MetodoPagoDto
    {
        [Required]
        public string Tipo { get; init; }

        [Required]
        [StringLength(100)]
        public string Proveedor { get; init; }

        [Required]
        [StringLength(100)]
        public string NumeroReferencia { get; init; }
    }
}
