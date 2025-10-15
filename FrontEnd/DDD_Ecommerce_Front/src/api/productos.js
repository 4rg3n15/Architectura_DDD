import { http } from "./http";

// Accepts various shapes and returns a flat array of products.
function normalizeToArray(res) {
  if (Array.isArray(res)) return res;
  if (res && typeof res === "object") {
    const candidates = [
      res.items,
      res.data,
      res.result,
      res.results,
      res.productos,
      res.products,
    ].filter(Array.isArray);
    if (candidates.length) return candidates[0];
    const vals = Object.values(res);
    if (vals.length && vals.every((v) => typeof v === "object")) return vals;
  }
  return null;
}

export async function fetchProductos() {
  const raw = await http.get("/productos");
  const arr = normalizeToArray(raw);
  if (!Array.isArray(arr))
    throw new Error("GET /productos must return an array");

  // Keep original backend names; UI will read them directly.
  return arr.map((p) => ({
    id: p.id ?? p._id ?? p.productoId ?? p.ProductoId ?? p.Id,
    nombre: p.nombre ?? p.Nombre ?? p.name ?? "Producto",
    descripcion: p.descripcion ?? p.Descripcion ?? p.description ?? "",
    precio: Number(p.precio ?? p.Precio ?? p.price ?? 0),
    stock: Number(p.stock ?? p.Stock ?? 0),
  }));
}
