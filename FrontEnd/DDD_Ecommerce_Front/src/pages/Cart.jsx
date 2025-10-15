import React from "react";
import Navbar from "../components/Navbar.jsx";
import { useCart } from "../context/CartContext.jsx";

export default function Cart({ onBack, onCheckout }) {
  const { cart, dispatch, subtotal } = useCart();
  const tax = subtotal * 0.19;
  const total = subtotal + tax;

  const update = (productoId, cantidad) => {
    dispatch({
      type: cantidad <= 0 ? "REMOVE_ITEM" : "UPDATE_QUANTITY",
      payload: cantidad <= 0 ? productoId : { productoId, cantidad },
    });
  };

  return (
    <>
      <Navbar onOpenCart={() => {}} />
      <div className="container">
        <button className="btn ghost" onClick={onBack}>
          ← Seguir comprando
        </button>
        <h2 style={{ margin: "1rem 0" }}>
          Tu Carrito ({cart.items.reduce((c, i) => c + i.cantidad, 0)} items)
        </h2>

        {cart.items.length === 0 ? (
          <p className="small">Tu carrito está vacío.</p>
        ) : (
          <div className="card" style={{ padding: "1rem" }}>
            {cart.items.map((it) => (
              <div
                key={it.productoId}
                className="space-between"
                style={{ padding: ".5rem 0", borderBottom: "1px solid #eee" }}
              >
                <div>
                  <div style={{ fontWeight: 700 }}>{it.nombreProducto}</div>
                  <div className="small">
                    ${it.precioUnitario.toFixed(2)} c/u
                  </div>
                </div>
                <div className="row">
                  <input
                    type="number"
                    className="input"
                    style={{ width: 72 }}
                    value={it.cantidad}
                    min={1}
                    onChange={(e) =>
                      update(
                        it.productoId,
                        Math.max(1, Number(e.target.value) || 1)
                      )
                    }
                  />
                  <button
                    className="btn ghost"
                    onClick={() =>
                      dispatch({ type: "REMOVE_ITEM", payload: it.productoId })
                    }
                  >
                    Quitar
                  </button>
                  <strong>
                    ${(it.cantidad * it.precioUnitario).toFixed(2)}
                  </strong>
                </div>
              </div>
            ))}
          </div>
        )}

        {cart.items.length > 0 && (
          <div className="card" style={{ padding: "1rem", marginTop: "1rem" }}>
            <div className="space-between">
              <span>Subtotal</span>
              <strong>${subtotal.toFixed(2)}</strong>
            </div>
            <div className="space-between small">
              <span>IVA (19%)</span>
              <span>${tax.toFixed(2)}</span>
            </div>
            <div
              className="space-between"
              style={{
                marginTop: ".5rem",
                borderTop: "1px solid #eee",
                paddingTop: ".5rem",
              }}
            >
              <span className="total">Total</span>
              <span className="total">${total.toFixed(2)}</span>
            </div>
            <button
              className="btn secondary"
              onClick={onCheckout}
              style={{ marginTop: "1rem" }}
            >
              Proceder a pagar
            </button>
          </div>
        )}
      </div>
    </>
  );
}
