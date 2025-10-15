import React, { createContext, useContext, useReducer, useState } from "react";
import {
  ShoppingCart,
  Plus,
  Minus,
  Trash2,
  CheckCircle,
  AlertCircle,
  Loader,
} from "lucide-react";

// ============================================================================
// HARDCODED PRODUCTS - Productos quemados directamente en el código
// ============================================================================
const PRODUCTOS = [
  {
    id: "550e8400-e29b-41d4-a716-446655440001",
    nombre: "Laptop Dell Inspiron 15 3000",
    descripcion:
      "Laptop Dell Inspiron 15 3000 con procesador Intel Core i5, 8GB RAM, 256GB SSD",
    precio: 2500000, // COP
    stock: 25,
    categoria: "Tecnología",
    codigo: "DELL-INS-15-3000",
  },
  {
    id: "550e8400-e29b-41d4-a716-446655440002",
    nombre: "Mouse Inalámbrico Logitech MX Master 3",
    descripcion:
      "Mouse inalámbrico premium con sensor de alta precisión y batería de larga duración",
    precio: 450000,
    stock: 150,
    categoria: "Accesorios",
    codigo: "LOG-MX-MASTER-3",
  },
  {
    id: "550e8400-e29b-41d4-a716-446655440003",
    nombre: "Teclado Mecánico Razer BlackWidow V3",
    descripcion:
      "Teclado mecánico gaming con switches Razer Green y retroiluminación RGB",
    precio: 680000,
    stock: 80,
    categoria: "Gaming",
    codigo: "RAZ-BW-V3",
  },
  {
    id: "550e8400-e29b-41d4-a716-446655440004",
    nombre: 'Monitor Samsung 24" Full HD',
    descripcion:
      "Monitor LED 24 pulgadas con resolución Full HD 1920x1080 y puerto HDMI",
    precio: 850000,
    stock: 40,
    categoria: "Tecnología",
    codigo: "SAM-24-FHD",
  },
  {
    id: "550e8400-e29b-41d4-a716-446655440005",
    nombre: "Audífonos Sony WH-1000XM4",
    descripcion:
      "Audífonos inalámbricos con cancelación de ruido y 30 horas de batería",
    precio: 1200000,
    stock: 30,
    categoria: "Audio",
    codigo: "SON-WH-1000XM4",
  },
];

// ============================================================================
// API SERVICE MODULE - Solo usa Pedidos API
// ============================================================================
const API_BASE_URL = "https://localhost:7235/api";

const apiService = {
  createOrder: async (clienteId, detalles) => {
    try {
      console.log("Creating order with:", { clienteId, detalles });
      const response = await fetch(`${API_BASE_URL}/pedidos`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ clienteId, detalles }),
      });
      if (!response.ok)
        throw new Error(`HTTP error! status: ${response.status}`);
      return await response.json();
    } catch (error) {
      console.error("Error creating order:", error);
      throw new Error(
        "Error al crear el pedido. Verifica los datos e intenta nuevamente."
      );
    }
  },

  confirmPayment: async (pedidoId, tipoPago, proveedor, numeroReferencia) => {
    try {
      console.log("Confirming payment:", {
        pedidoId,
        tipoPago,
        proveedor,
        numeroReferencia,
      });
      const response = await fetch(`${API_BASE_URL}/pedidos/${pedidoId}/pago`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({
          pedidoId,
          tipoPago,
          proveedor,
          numeroReferencia,
        }),
      });
      if (!response.ok)
        throw new Error(`HTTP error! status: ${response.status}`);
      return await response.json();
    } catch (error) {
      console.error("Error confirming payment:", error);
      throw new Error("Error al confirmar el pago. Intenta nuevamente.");
    }
  },
};

// ============================================================================
// CART CONTEXT & STATE MANAGEMENT
// ============================================================================
const CartContext = createContext();

const cartReducer = (state, action) => {
  switch (action.type) {
    case "ADD_ITEM": {
      const existingItem = state.items.find(
        (item) => item.productoId === action.payload.productoId
      );
      if (existingItem) {
        return {
          ...state,
          items: state.items.map((item) =>
            item.productoId === action.payload.productoId
              ? { ...item, cantidad: item.cantidad + action.payload.cantidad }
              : item
          ),
        };
      }
      return { ...state, items: [...state.items, action.payload] };
    }
    case "UPDATE_QUANTITY":
      return {
        ...state,
        items: state.items.map((item) =>
          item.productoId === action.payload.productoId
            ? { ...item, cantidad: Math.max(1, action.payload.cantidad) }
            : item
        ),
      };
    case "REMOVE_ITEM":
      return {
        ...state,
        items: state.items.filter((item) => item.productoId !== action.payload),
      };
    case "CLEAR_CART":
      return { ...state, items: [] };
    default:
      return state;
  }
};

const CartProvider = ({ children }) => {
  const [cart, dispatch] = useReducer(cartReducer, { items: [] });
  return (
    <CartContext.Provider value={{ cart, dispatch }}>
      {children}
    </CartContext.Provider>
  );
};

const useCart = () => {
  const context = useContext(CartContext);
  if (!context) throw new Error("useCart must be used within CartProvider");
  return context;
};

// ============================================================================
// PRODUCT LISTING MODULE
// ============================================================================
const ProductCard = ({ product, onAddToCart }) => {
  const [quantity, setQuantity] = useState(1);

  const handleAdd = () => {
    onAddToCart({
      productoId: product.id,
      nombreProducto: product.nombre,
      precioUnitario: product.precio,
      cantidad: quantity,
    });
    setQuantity(1);
  };

  return (
    <div className="bg-white rounded-lg shadow-md overflow-hidden hover:shadow-lg transition-shadow">
      <div className="h-48 bg-gradient-to-br from-blue-100 to-blue-50 flex items-center justify-center">
        <div className="text-6xl">📦</div>
      </div>
      <div className="p-4">
        <h3 className="text-lg font-semibold text-gray-800 truncate">
          {product.nombre}
        </h3>
        <p className="text-sm text-gray-600 mt-1 line-clamp-2">
          {product.descripcion}
        </p>
        <div className="mt-4 flex items-center justify-between">
          <span className="text-2xl font-bold text-green-600">
            ${product.precio.toFixed(2)}
          </span>
          <span
            className={`text-xs px-2 py-1 rounded font-semibold ${
              product.stock > 0
                ? "bg-green-100 text-green-800"
                : "bg-red-100 text-red-800"
            }`}
          >
            Stock: {product.stock}
          </span>
        </div>
        <div className="mt-4 flex items-center gap-2">
          <button
            onClick={() => setQuantity(Math.max(1, quantity - 1))}
            className="flex-1 bg-gray-200 hover:bg-gray-300 text-gray-700 py-2 rounded font-semibold transition"
          >
            <Minus size={18} className="mx-auto" />
          </button>
          <span className="flex-1 text-center font-semibold text-gray-800">
            {quantity}
          </span>
          <button
            onClick={() => setQuantity(quantity + 1)}
            className="flex-1 bg-gray-200 hover:bg-gray-300 text-gray-700 py-2 rounded font-semibold transition"
          >
            <Plus size={18} className="mx-auto" />
          </button>
        </div>
        <button
          onClick={handleAdd}
          disabled={product.stock === 0}
          className="w-full mt-4 bg-blue-600 hover:bg-blue-700 disabled:bg-gray-400 text-white py-2 rounded-lg font-semibold transition flex items-center justify-center gap-2"
        >
          <ShoppingCart size={18} />
          Agregar al carrito
        </button>
      </div>
    </div>
  );
};

const ProductList = () => {
  const { dispatch } = useCart();

  const handleAddToCart = (product) => {
    dispatch({ type: "ADD_ITEM", payload: product });
  };

  return (
    <div>
      <h2 className="text-3xl font-bold text-gray-800 mb-8">
        Catálogo de Productos
      </h2>
      <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-6">
        {PRODUCTOS.map((product) => (
          <ProductCard
            key={product.id}
            product={product}
            onAddToCart={handleAddToCart}
          />
        ))}
      </div>
    </div>
  );
};

// ============================================================================
// CART MODULE
// ============================================================================
const CartItem = ({ item, onUpdate, onRemove }) => {
  return (
    <div className="flex items-center gap-4 bg-white p-4 rounded-lg border border-gray-200">
      <div className="flex-1">
        <h4 className="font-semibold text-gray-800">{item.nombreProducto}</h4>
        <p className="text-sm text-gray-600">
          ${item.precioUnitario.toFixed(2)}
        </p>
      </div>
      <div className="flex items-center gap-2 bg-gray-100 rounded-lg p-2">
        <button
          onClick={() => onUpdate(item.productoId, item.cantidad - 1)}
          className="bg-white hover:bg-gray-200 px-3 py-1 rounded transition"
        >
          <Minus size={16} />
        </button>
        <span className="w-8 text-center font-semibold text-gray-800">
          {item.cantidad}
        </span>
        <button
          onClick={() => onUpdate(item.productoId, item.cantidad + 1)}
          className="bg-white hover:bg-gray-200 px-3 py-1 rounded transition"
        >
          <Plus size={16} />
        </button>
      </div>
      <div className="text-right min-w-20">
        <p className="font-semibold text-gray-800">
          ${(item.cantidad * item.precioUnitario).toFixed(2)}
        </p>
      </div>
      <button
        onClick={() => onRemove(item.productoId)}
        className="bg-red-100 hover:bg-red-200 text-red-600 p-2 rounded transition"
      >
        <Trash2 size={18} />
      </button>
    </div>
  );
};

const CartSummary = ({ items }) => {
  const subtotal = items.reduce(
    (sum, item) => sum + item.cantidad * item.precioUnitario,
    0
  );
  const impuestos = subtotal * 0.19;
  const total = subtotal + impuestos;

  return (
    <div className="bg-white rounded-lg shadow-md p-6 sticky top-6">
      <h3 className="text-xl font-bold text-gray-800 mb-4">
        Resumen del Carrito
      </h3>
      <div className="space-y-3 border-b border-gray-200 pb-4">
        <div className="flex justify-between text-gray-700">
          <span>Subtotal:</span>
          <span>${subtotal.toFixed(2)}</span>
        </div>
        <div className="flex justify-between text-gray-700">
          <span>IVA (19%):</span>
          <span>${impuestos.toFixed(2)}</span>
        </div>
        <div className="flex justify-between text-sm text-gray-500">
          <span>Items:</span>
          <span>{items.length}</span>
        </div>
      </div>
      <div className="mt-4 flex justify-between text-xl font-bold text-gray-900">
        <span>Total:</span>
        <span className="text-green-600">${total.toFixed(2)}</span>
      </div>
    </div>
  );
};

const CartView = ({ onCheckout }) => {
  const { cart, dispatch } = useCart();

  const handleUpdateQuantity = (productoId, cantidad) => {
    if (cantidad <= 0) {
      dispatch({ type: "REMOVE_ITEM", payload: productoId });
    } else {
      dispatch({ type: "UPDATE_QUANTITY", payload: { productoId, cantidad } });
    }
  };

  return (
    <div className="space-y-6">
      <h2 className="text-3xl font-bold text-gray-800">
        Tu Carrito de Compras
      </h2>
      {cart.items.length === 0 ? (
        <div className="bg-blue-50 border-2 border-blue-200 rounded-lg p-12 text-center">
          <ShoppingCart size={56} className="mx-auto text-blue-400 mb-4" />
          <p className="text-gray-600 text-lg">Tu carrito está vacío</p>
          <p className="text-gray-500 text-sm mt-2">
            Agrega productos para comenzar tu compra
          </p>
        </div>
      ) : (
        <div className="grid grid-cols-1 lg:grid-cols-3 gap-6">
          <div className="lg:col-span-2 space-y-4">
            {cart.items.map((item) => (
              <CartItem
                key={item.productoId}
                item={item}
                onUpdate={handleUpdateQuantity}
                onRemove={(id) =>
                  dispatch({ type: "REMOVE_ITEM", payload: id })
                }
              />
            ))}
          </div>
          <div>
            <CartSummary items={cart.items} />
            <button
              onClick={onCheckout}
              className="w-full mt-6 bg-green-600 hover:bg-green-700 text-white py-3 rounded-lg font-bold text-lg transition"
            >
              Proceder a Pagar
            </button>
          </div>
        </div>
      )}
    </div>
  );
};

// ============================================================================
// CHECKOUT MODULE
// ============================================================================
const CheckoutForm = ({ onSubmit, loading }) => {
  const [formData, setFormData] = useState({
    clienteId: "",
    nombreCliente: "",
    email: "",
    tipoPago: "Tarjeta",
    proveedor: "",
    numeroReferencia: "",
  });

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData((prev) => ({ ...prev, [name]: value }));
  };

  const handleSubmit = () => {
    if (
      !formData.clienteId ||
      !formData.nombreCliente ||
      !formData.email ||
      !formData.proveedor ||
      !formData.numeroReferencia
    ) {
      alert("Por favor completa todos los campos");
      return;
    }
    onSubmit(formData);
  };

  return (
    <div className="space-y-6">
      <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
        <div>
          <label className="block text-sm font-semibold text-gray-700 mb-2">
            Nombre completo
          </label>
          <input
            type="text"
            name="nombreCliente"
            placeholder="Juan Pérez"
            value={formData.nombreCliente}
            onChange={handleChange}
            className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500"
          />
        </div>
        <div>
          <label className="block text-sm font-semibold text-gray-700 mb-2">
            Email
          </label>
          <input
            type="email"
            name="email"
            placeholder="juan@example.com"
            value={formData.email}
            onChange={handleChange}
            className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500"
          />
        </div>
      </div>

      <div>
        <label className="block text-sm font-semibold text-gray-700 mb-2">
          ID Cliente (UUID)
        </label>
        <input
          type="text"
          name="clienteId"
          placeholder="550e8400-e29b-41d4-a716-446655440000"
          value={formData.clienteId}
          onChange={handleChange}
          className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500"
        />
        <p className="text-xs text-gray-500 mt-1">
          Formato: xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx
        </p>
      </div>

      <div className="bg-gray-50 p-6 rounded-lg space-y-4 border border-gray-200">
        <h4 className="font-bold text-gray-800 text-lg">Información de Pago</h4>

        <div>
          <label className="block text-sm font-semibold text-gray-700 mb-2">
            Método de Pago
          </label>
          <select
            name="tipoPago"
            value={formData.tipoPago}
            onChange={handleChange}
            className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500"
          >
            <option>Tarjeta</option>
            <option>Transferencia</option>
            <option>Efectivo</option>
          </select>
        </div>

        <div>
          <label className="block text-sm font-semibold text-gray-700 mb-2">
            Proveedor
          </label>
          <input
            type="text"
            name="proveedor"
            placeholder="Ej: Visa, Bancolombia, Nequi"
            value={formData.proveedor}
            onChange={handleChange}
            className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500"
          />
        </div>

        <div>
          <label className="block text-sm font-semibold text-gray-700 mb-2">
            Número de referencia
          </label>
          <input
            type="text"
            name="numeroReferencia"
            placeholder="Ej: TXN-123456789"
            value={formData.numeroReferencia}
            onChange={handleChange}
            className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500"
          />
        </div>
      </div>

      <button
        onClick={handleSubmit}
        disabled={loading}
        className="w-full bg-blue-600 hover:bg-blue-700 disabled:bg-gray-400 text-white py-3 rounded-lg font-bold transition flex items-center justify-center gap-2"
      >
        {loading ? (
          <>
            <Loader size={18} className="animate-spin" />
            Procesando...
          </>
        ) : (
          <>
            <ShoppingCart size={18} />
            Completar Compra
          </>
        )}
      </button>
    </div>
  );
};

// ============================================================================
// CONFIRMATION MODULE
// ============================================================================
const ConfirmationView = ({ order, onNewOrder }) => {
  return (
    <div className="max-w-md mx-auto bg-white rounded-lg shadow-lg p-8 text-center">
      <CheckCircle size={64} className="mx-auto text-green-600 mb-4" />
      <h2 className="text-2xl font-bold text-gray-800 mb-2">
        ¡Compra Exitosa!
      </h2>
      <p className="text-gray-600 mb-6">
        Tu pedido ha sido procesado correctamente
      </p>

      <div className="bg-gray-50 rounded-lg p-4 mb-6 text-left space-y-2 border border-gray-200">
        <p className="text-sm">
          <span className="font-semibold">Número de Pedido:</span>{" "}
          {order?.numeroPedido || "N/A"}
        </p>
        <p className="text-sm">
          <span className="font-semibold">Total Pagado:</span> $
          {(order?.montoPagado || 0).toFixed(2)}
        </p>
        <p className="text-sm">
          <span className="font-semibold">Estado:</span>{" "}
          <span className="text-green-600 font-semibold">
            {order?.estado || "Procesando"}
          </span>
        </p>
      </div>

      <button
        onClick={onNewOrder}
        className="w-full bg-blue-600 hover:bg-blue-700 text-white py-3 rounded-lg font-bold transition"
      >
        Realizar Otra Compra
      </button>
    </div>
  );
};

// ============================================================================
// MAIN APPLICATION
// ============================================================================
function AppContent() {
  const [currentStep, setCurrentStep] = useState("products");
  const [selectedOrder, setSelectedOrder] = useState(null);
  const [error, setError] = useState(null);
  const [loading, setLoading] = useState(false);
  const { cart, dispatch: cartDispatch } = useCart();

  const handleCheckout = () => {
    setCurrentStep("checkout");
  };

  const handleSubmitOrder = async (formData) => {
    setLoading(true);
    setError(null);

    try {
      const detalles = cart.items.map((item) => ({
        productoId: item.productoId,
        nombreProducto: item.nombreProducto,
        cantidad: item.cantidad,
        precioUnitario: item.precioUnitario,
      }));

      const orderResponse = await apiService.createOrder(
        formData.clienteId,
        detalles
      );
      console.log("Order created:", orderResponse);

      const paymentResponse = await apiService.confirmPayment(
        orderResponse.pedidoId,
        formData.tipoPago,
        formData.proveedor,
        formData.numeroReferencia
      );

      console.log("Payment confirmed:", paymentResponse);
      setSelectedOrder(paymentResponse);
      cartDispatch({ type: "CLEAR_CART" });
      setCurrentStep("confirmation");
    } catch (err) {
      console.error("Order error:", err);
      setError(err.message || "Error al procesar la compra");
    } finally {
      setLoading(false);
    }
  };

  const handleNewOrder = () => {
    setCurrentStep("products");
    setSelectedOrder(null);
    setError(null);
  };

  return (
    <div className="min-h-screen bg-gradient-to-br from-blue-50 to-indigo-100">
      <nav className="bg-white shadow-md">
        <div className="max-w-7xl mx-auto px-4 py-4 flex items-center justify-between">
          <div className="flex items-center gap-2">
            <ShoppingCart size={28} className="text-blue-600" />
            <h1 className="text-2xl font-bold text-gray-800">TechStore</h1>
          </div>
          <div className="flex gap-4">
            {currentStep !== "confirmation" && (
              <>
                <button
                  onClick={() => setCurrentStep("products")}
                  className={`px-4 py-2 rounded-lg font-semibold transition ${
                    currentStep === "products"
                      ? "bg-blue-600 text-white"
                      : "bg-gray-200 text-gray-800 hover:bg-gray-300"
                  }`}
                >
                  Productos
                </button>
                <button
                  onClick={() => setCurrentStep("cart")}
                  className={`px-4 py-2 rounded-lg font-semibold transition ${
                    currentStep === "cart"
                      ? "bg-blue-600 text-white"
                      : "bg-gray-200 text-gray-800 hover:bg-gray-300"
                  }`}
                >
                  Mi Carrito ({cart.items.length})
                </button>
              </>
            )}
          </div>
        </div>
      </nav>

      {error && (
        <div className="max-w-7xl mx-auto mt-4 mx-4 bg-red-50 border-2 border-red-300 rounded-lg p-4 flex gap-3">
          <AlertCircle size={24} className="text-red-600 flex-shrink-0" />
          <div>
            <h3 className="font-semibold text-red-800">Error</h3>
            <p className="text-red-700">{error}</p>
          </div>
        </div>
      )}

      <main className="max-w-7xl mx-auto px-4 py-8">
        {currentStep === "products" && <ProductList />}
        {currentStep === "cart" && <CartView onCheckout={handleCheckout} />}
        {currentStep === "checkout" && (
          <div className="max-w-2xl mx-auto">
            <h2 className="text-3xl font-bold text-gray-800 mb-6">
              Checkout - Completa tu Compra
            </h2>
            <CheckoutForm onSubmit={handleSubmitOrder} loading={loading} />
          </div>
        )}
        {currentStep === "confirmation" && (
          <ConfirmationView order={selectedOrder} onNewOrder={handleNewOrder} />
        )}
      </main>
    </div>
  );
}

export default function App() {
  return (
    <CartProvider>
      <AppContent />
    </CartProvider>
  );
}
