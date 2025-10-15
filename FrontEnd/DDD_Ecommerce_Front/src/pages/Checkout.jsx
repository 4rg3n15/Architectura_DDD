import React, { useState } from "react";
import Navbar from "../components/Navbar.jsx";
import { useCart } from "../context/CartContext.jsx";
import { crearPedido, confirmarPago } from "../api/pedidos.js";

export default function Checkout({ onBack, onConfirmed }) {
  const { cart, subtotal, dispatch } = useCart();
  const [form, setForm] = useState({
    clienteId: "",
    tipoPago: "Tarjeta",
    proveedor: "",
    numeroReferencia: "",
  });
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");
  const tax = subtotal * 0.19;
  const total = subtotal + tax;

  async function handlePay() {
    setError("");
    setLoading(true);
    try {
      const detalles = cart.items.map((it) => ({
        productoId: it.productoId,
        nombreProducto: it.nombreProducto,
        cantidad: it.cantidad,
        precioUnitario: it.precioUnitario,
      }));
      const creado = await crearPedido({ clienteId: form.clienteId, detalles });
      const pedidoId =
        creado.pedidoId || creado.id || creado.PedidoId || creado.Id;
      if (!pedidoId) throw new Error("El backend no retornó pedidoId");

      await confirmarPago(pedidoId, {
        tipoPago: form.tipoPago,
        proveedor: form.proveedor || "VISA",
        numeroReferencia: form.numeroReferencia || `WEB-${Date.now()}`,
      });

      dispatch({ type: "CLEAR_CART" });
      onConfirmed(pedidoId);
    } catch (e) {
      setError(e.message || "Error en el checkout");
    } finally {
      setLoading(false);
    }
  }

  return (
    <>
      <Navbar onOpenCart={() => {}} />
      <div className="container">
        <button className="btn ghost" onClick={onBack}>
          ← Volver al carrito
        </button>
        <h2 style={{ margin: "1rem 0" }}>Checkout</h2>

        <div className="grid" style={{ gridTemplateColumns: "1fr 1fr" }}>
          <div className="card" style={{ padding: "1rem" }}>
            <strong>Resumen</strong>
            {cart.items.map((it) => (
              <div
                key={it.productoId}
                className="space-between"
                style={{ padding: ".4rem 0" }}
              >
                <span>
                  {it.nombreProducto} × {it.cantidad}
                </span>
                <span>${(it.precioUnitario * it.cantidad).toFixed(2)}</span>
              </div>
            ))}
            <div
              className="space-between"
              style={{
                marginTop: ".5rem",
                borderTop: "1px solid #eee",
                paddingTop: ".5rem",
              }}
            >
              <span>Subtotal</span>
              <strong>${subtotal.toFixed(2)}</strong>
            </div>
            <div className="space-between small">
              <span>IVA (19%)</span>
              <span>${tax.toFixed(2)}</span>
            </div>
            <div className="space-between" style={{ marginTop: ".5rem" }}>
              <span className="total">Total</span>
              <span className="total">${total.toFixed(2)}</span>
            </div>
          </div>

          <div className="card" style={{ padding: "1rem" }}>
            <strong>Pago</strong>
            <input
              className="input"
              placeholder="ID Cliente (UUID)"
              value={form.clienteId}
              onChange={(e) =>
                setForm((f) => ({ ...f, clienteId: e.target.value }))
              }
              style={{ marginTop: ".5rem" }}
            />
            <select
              className="input"
              value={form.tipoPago}
              onChange={(e) =>
                setForm((f) => ({ ...f, tipoPago: e.target.value }))
              }
              style={{ marginTop: ".5rem" }}
            >
              <option>Tarjeta</option>
              <option>Transferencia</option>
              <option>Efectivo</option>
            </select>
            <input
              className="input"
              placeholder="Proveedor (VISA, Bancolombia…)"
              value={form.proveedor}
              onChange={(e) =>
                setForm((f) => ({ ...f, proveedor: e.target.value }))
              }
              style={{ marginTop: ".5rem" }}
            />
            <input
              className="input"
              placeholder="Número de referencia"
              value={form.numeroReferencia}
              onChange={(e) =>
                setForm((f) => ({ ...f, numeroReferencia: e.target.value }))
              }
              style={{ marginTop: ".5rem" }}
            />

            {error && (
              <p
                className="small"
                style={{ color: "#b91c1c", marginTop: ".5rem" }}
              >
                Error: {error}
              </p>
            )}
            <button
              className="btn secondary"
              disabled={loading || cart.items.length === 0}
              onClick={handlePay}
              style={{ marginTop: "1rem" }}
            >
              {loading ? "Procesando…" : `Pagar $${total.toFixed(2)}`}
            </button>
          </div>
        </div>
      </div>
    </>
  );
}
