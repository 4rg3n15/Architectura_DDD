using System;
using System.Collections.Generic;

namespace Arquitectura_DDD.Application.DTOs
{
    public class PedidoDto
    {
        // DTOs de Entrada
        public record CrearPedidoRequest(
            Guid ClienteId,
            string Calle,
            string Ciudad,
            string Departamento,
            string CodigoPostal,
            string Referencias,
            List<DetallePedidoRequest> Detalles);

        public record DetallePedidoRequest(
            Guid ProductoId,
            string NombreProducto,
            int Cantidad,
            decimal PrecioUnitario);

        public record AplicarPagoRequest(
            string TipoPago,
            string ProveedorPago,
            string NumeroReferencia);

        public record GenerarFacturaRequest(
            string NitCliente);

        public record ActualizarEstadoRequest(
            string NuevoEstado,
            string? EmpresaMensajeria = null,
            string? NumeroGuia = null,
            string? PersonaRecibe = null,
            string? MotivoCancelacion = null);

        // DTOs de Salida
        public record PedidoResponse(
            Guid Id,
            Guid ClienteId,
            string Estado,
            decimal Total,
            string DireccionEntrega,
            DateTime FechaCreacion,
            List<DetallePedidoResponse> Detalles,
            string? MetodoPago = null,
            string? NumeroFactura = null);

        public record DetallePedidoResponse(
            string ProductoId,
            string NombreProducto,
            int Cantidad,
            decimal PrecioUnitario,
            decimal Subtotal);

        

        public record FacturaResponse(
            string NumeroFactura,
            DateTime FechaEmision,
            string NitCliente,
            decimal ValorTotal);
    }
}
