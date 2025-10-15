import { http } from "./http";

export async function crearPedido({ clienteId, detalles }) {
  // detalles: [{ productoId, nombreProducto, cantidad, precioUnitario }]
  return http.post("/pedidos", { clienteId, detalles });
}

export async function confirmarPago(
  pedidoId,
  { tipoPago, proveedor, numeroReferencia }
) {
  return http.post(`/pedidos/${pedidoId}/pago`, {
    pedidoId,
    tipoPago,
    proveedor,
    numeroReferencia,
  });
}
