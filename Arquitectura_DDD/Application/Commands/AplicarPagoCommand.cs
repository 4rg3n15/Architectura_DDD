using Arquitectura_DDD.Application.Common;

namespace Arquitectura_DDD.Application.Commands
{
    public sealed record AplicarPagoCommand : IRequest<Unit>
    {
        public Guid PedidoId { get; init; }
        public string TipoPago { get; init; }
        public string ProveedorPago { get; init; }
        public string NumeroReferencia { get; init; }
    }
}
