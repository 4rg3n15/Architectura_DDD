import React, { useState } from "react";
import { useCart } from "../context/CartContext.jsx";

export default function ProductCard({ product }) {
  const [qty, setQty] = useState(1);
  const { dispatch } = useCart();

  const add = () => {
    dispatch({
      type: "ADD_ITEM",
      payload: {
        productoId: product.id,
        nombreProducto: product.nombre,
        precioUnitario: product.precio,
        cantidad: qty,
      },
    });
    setQty(1);
  };

  return (
    <div className="card">
      <div className="card-body">
        <div className="space-between">
          <strong>{product.nombre}</strong>
          <span className="small">Stock: {product.stock ?? "-"}</span>
        </div>
        <span className="small">
          {product.descripcion || "Sin descripción"}
        </span>
        <div className="space-between" style={{ marginTop: ".5rem" }}>
          <span className="price">${product.precio.toFixed(2)}</span>
          <div className="row" style={{ gap: ".25rem" }}>
            <button
              className="btn ghost"
              onClick={() => setQty(Math.max(1, qty - 1))}
            >
              -
            </button>
            <input
              className="input"
              style={{ width: 64, textAlign: "center" }}
              type="number"
              value={qty}
              min={1}
              onChange={(e) => setQty(Math.max(1, Number(e.target.value) || 1))}
            />
            <button className="btn ghost" onClick={() => setQty(qty + 1)}>
              +
            </button>
          </div>
        </div>
        <button className="btn" style={{ marginTop: ".5rem" }} onClick={add}>
          Agregar al carrito
        </button>
      </div>
    </div>
  );
}
