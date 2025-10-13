using Arquitectura_DDD.Application.Common;

namespace Arquitectura_DDD.Application.Commands
{
    public class ActualizarEstadoCommand : IRequest<Unit>
    {
        public Guid PedidoId { get; init; }
        public string NuevoEstado { get; init; }
        public string EmpresaMensajeria { get; init; }
        public string NumeroGuia { get; init; }
        public string PersonaRecibe { get; init; }
        public string MotivoCancelacion { get; init; }
    }
}
