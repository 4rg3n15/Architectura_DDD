using Arquitectura_DDD.Core.Aggregates;

namespace Arquitectura_DDD.Core.Interfaces.InterfacesApplicacion
{
    public interface INotificacionClientesService
    {
        Task NotificarPedidoCreado(PedidoVenta pedido);
        Task NotificarPedidoPagado(PedidoVenta pedido);
        Task NotificarPedidoEnviado(PedidoVenta pedido);
        Task NotificarPedidoEntregado(PedidoVenta pedido);
        Task NotificarPedidoCancelado(PedidoVenta pedido, string motivo);
    }
}
