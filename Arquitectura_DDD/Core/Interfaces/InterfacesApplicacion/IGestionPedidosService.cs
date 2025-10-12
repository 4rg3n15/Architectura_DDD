using Arquitectura_DDD.Core.Aggregates;
using Arquitectura_DDD.Core.ValueObjects;

namespace Arquitectura_DDD.Core.Interfaces.InterfacesApplicacion
{
    public interface IGestionPedidosService
    {
        Task<PedidoVenta> CrearPedido(Guid clienteId, DireccionEntrega direccionEntrega);
        Task AgregarProductoAPedido(Guid pedidoId, string productoId, string nombreProducto, int cantidad, decimal precioUnitario);
        Task AplicarMetodoPago(Guid pedidoId, MetodoPago metodoPago);
        Task<bool> ValidarDisponibilidadProducto(string productoId, int cantidad);
    }
}
