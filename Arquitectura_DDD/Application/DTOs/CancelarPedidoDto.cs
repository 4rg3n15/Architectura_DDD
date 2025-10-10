using System.ComponentModel.DataAnnotations;

namespace Arquitectura_DDD.Application.DTOs
{
    public sealed record CancelarPedidoDto
    {
        [Required]
        public string Motivo { get; init; }
    }
}
