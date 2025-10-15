import React from "react";
import { ShoppingCart } from "lucide-react";
import { useCart } from "../context/CartContext.jsx";

export default function Navbar({ onOpenCart }) {
  const { count } = useCart();
  return (
    <div className="nav">
      <div className="container nav-inner space-between">
        <div className="row" style={{ gap: "0.5rem" }}>
          <span style={{ fontWeight: 800 }}>TechStore</span>
          <span className="badge">Mongo + DDD</span>
        </div>
        <button className="btn" onClick={onOpenCart}>
          <span className="row" style={{ gap: ".5rem" }}>
            <ShoppingCart size={18} /> Cart ({count})
          </span>
        </button>
      </div>
    </div>
  );
}
