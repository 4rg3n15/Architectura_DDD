import React, { useEffect, useState } from "react";
import Navbar from "../components/Navbar.jsx";
import ProductCard from "../components/ProductCard.jsx";
import { fetchProductos } from "../api/productos.js";

export default function Products({ onOpenCart }) {
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");
  const [products, setProducts] = useState([]);

  useEffect(() => {
    (async () => {
      try {
        const data = await fetchProductos();
        setProducts(data);
      } catch (e) {
        setError(e.message || "No se pudieron cargar los productos");
      } finally {
        setLoading(false);
      }
    })();
  }, []);

  return (
    <>
      <Navbar onOpenCart={onOpenCart} />
      <div className="container">
        <h2 style={{ margin: "1rem 0" }}>Nuestros Productos</h2>
        {loading && <p className="small">Cargando productos…</p>}
        {error && (
          <p className="small" style={{ color: "#b91c1c" }}>
            Error: {error}
          </p>
        )}
        {!loading && !error && (
          <div className="grid">
            {products.map((p) => (
              <ProductCard key={p.id} product={p} />
            ))}
          </div>
        )}
      </div>
    </>
  );
}
