import React, { useState } from "react";
import { CartProvider } from "./context/CartContext.jsx";
import Products from "./pages/Products.jsx";
import Cart from "./pages/Cart.jsx";
import Checkout from "./pages/Checkout.jsx";
import Confirmation from "./pages/Confirmation.jsx";
import "./styles.css";

export default function App() {
  const [view, setView] = useState("products");
  const [orderId, setOrderId] = useState("");
  return (
    <CartProvider>
      {view === "products" && <Products onOpenCart={() => setView("cart")} />}
      {view === "cart" && (
        <Cart
          onBack={() => setView("products")}
          onCheckout={() => setView("checkout")}
        />
      )}
      {view === "checkout" && (
        <Checkout
          onBack={() => setView("cart")}
          onConfirmed={(id) => {
            setOrderId(id);
            setView("confirmation");
          }}
        />
      )}
      {view === "confirmation" && (
        <Confirmation orderId={orderId} onHome={() => setView("products")} />
      )}
    </CartProvider>
  );
}
