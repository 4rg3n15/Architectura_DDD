using System;
using System.ComponentModel.DataAnnotations;

namespace Arquitectura_DDD.Application.DTOs
{
    public class CancelarPedidoRequestDto
    {
        [Required]
        public Guid PedidoId { get; set; }

        [Required]
        [StringLength(500, MinimumLength = 10, ErrorMessage = "El motivo debe tener entre 10 y 500 caracteres")]
        public string Motivo { get; set; } = string.Empty;
    }
}
