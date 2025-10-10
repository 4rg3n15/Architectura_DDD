namespace Arquitectura_DDD.Application.Commands
{
    public class CrearPedidoCommand : IRequest<Guid>
    {
        public Guid ClienteId { get; init; }
        public List<DetallePedidoDto> Detalles { get; init; } = new();
        public MetodoPagoDto MetodoPago { get; init; }
    }


}
