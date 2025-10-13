using Arquitectura_DDD.Application.Common;
using static Arquitectura_DDD.Application.DTOs.PedidoDto;

namespace Arquitectura_DDD.Application.Commands
{
    public class GenerarFacturaCommand : IRequest<FacturaResponse>
    {
        public Guid PedidoId { get; init; }
        public string NitCliente { get; init; }
    }
}
