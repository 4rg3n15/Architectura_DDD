import React, { useEffect } from "react";
import Navbar from "../components/Navbar.jsx";
import { useCart } from "../context/CartContext.jsx";

export default function Confirmation({ orderId, onHome }) {
  const { dispatch } = useCart();
  useEffect(() => {
    dispatch({ type: "CLEAR_CART" });
  }, [dispatch]);
  const number = orderId || `PED-${Date.now()}`;

  return (
    <>
      <Navbar onOpenCart={() => {}} />
      <div
        className="container"
        style={{ display: "grid", placeItems: "center", minHeight: "60vh" }}
      >
        <div className="card" style={{ padding: "2rem", maxWidth: 500 }}>
          <h2>¡Compra Exitosa!</h2>
          <p className="small">Tu pedido fue procesado correctamente.</p>
          <div className="row" style={{ justifyContent: "space-between" }}>
            <span className="small">Número de Pedido</span>
            <strong>{number}</strong>
          </div>
          <button
            className="btn"
            style={{ marginTop: "1rem" }}
            onClick={onHome}
          >
            Seguir comprando
          </button>
        </div>
      </div>
    </>
  );
}
