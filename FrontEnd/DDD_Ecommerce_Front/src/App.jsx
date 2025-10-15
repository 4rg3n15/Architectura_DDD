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
import "./styles.css";

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
  const count = cart.items.reduce((total, item) => total + item.cantidad, 0);
  return (
    <CartContext.Provider value={{ cart, dispatch, count }}>
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

  const getStockClass = () => {
    if (product.stock > 50) return "high";
    if (product.stock > 20) return "medium";
    if (product.stock > 0) return "low";
    return "out";
  };

  const getProductEmoji = () => {
    switch (product.categoria) {
      case "Tecnología": return "💻";
      case "Accesorios": return "🖱️";
      case "Gaming": return "⌨️";
      case "Audio": return "🎧";
      default: return "📦";
    }
  };

  return (
    <div className="product-card">
      <div className="product-image">
        <div className="product-emoji">{getProductEmoji()}</div>
        <div className={`stock-badge ${getStockClass()}`}>
          {product.stock > 0 ? `${product.stock} disponibles` : "Agotado"}
        </div>
      </div>
      <div className="product-info">
        <h3 className="product-name">{product.nombre}</h3>
        <p className="product-description">{product.descripcion}</p>
        
        <div className="product-price-section">
          <span className="product-price">${product.precio.toLocaleString()}</span>
          <span className="product-category">{product.categoria}</span>
        </div>

        <div className="quantity-controls">
          <button
            onClick={() => setQuantity(Math.max(1, quantity - 1))}
            className="qty-btn"
          >
            <Minus size={16} />
          </button>
          <span className="qty-display">{quantity}</span>
          <button
            onClick={() => setQuantity(quantity + 1)}
            className="qty-btn"
          >
            <Plus size={16} />
          </button>
        </div>

        <button
          onClick={handleAdd}
          disabled={product.stock === 0}
          className="add-to-cart-btn"
        >
          <ShoppingCart size={20} />
          {product.stock === 0 ? "Agotado" : "Agregar al carrito"}
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
    <div className="container">
      <div className="page-header">
        <h2 className="page-title">Catálogo de Productos</h2>
        <p className="page-subtitle">
          Descubre nuestra selección premium de tecnología y accesorios de última generación
        </p>
        <div className="benefits">
          <div className="benefit-item">
            <div className="benefit-dot green"></div>
            Envío gratis
          </div>
          <div className="benefit-item">
            <div className="benefit-dot blue"></div>
            Garantía extendida
          </div>
          <div className="benefit-item">
            <div className="benefit-dot purple"></div>
            Soporte 24/7
          </div>
        </div>
      </div>
      <div className="product-grid">
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
    <div className="cart-item">
      <div className="cart-item-content">
        <div className="cart-item-image">
          <span>📦</span>
        </div>
        
        <div className="cart-item-info">
          <h4 className="cart-item-name">{item.nombreProducto}</h4>
          <p className="cart-item-price">
            Precio unitario: <span className="price">${item.precioUnitario.toLocaleString()}</span>
          </p>
        </div>

        <div className="cart-item-controls">
          <div className="cart-qty-controls">
            <button
              onClick={() => onUpdate(item.productoId, item.cantidad - 1)}
              className="cart-qty-btn"
            >
              <Minus size={14} />
            </button>
            <span className="cart-qty-display">{item.cantidad}</span>
            <button
              onClick={() => onUpdate(item.productoId, item.cantidad + 1)}
              className="cart-qty-btn"
            >
              <Plus size={14} />
            </button>
          </div>

          <div className="cart-item-total">
            <p className="cart-item-total-price">
              ${(item.cantidad * item.precioUnitario).toLocaleString()}
            </p>
          </div>

          <button
            onClick={() => onRemove(item.productoId)}
            className="cart-remove-btn"
          >
            <Trash2 size={16} />
          </button>
        </div>
      </div>
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
    <div className="cart-summary">
      <h3 className="cart-summary-title">Resumen del Carrito</h3>
      
      <div className="cart-summary-items">
        <div className="cart-summary-item">
          <span className="cart-summary-label">Subtotal:</span>
          <span className="cart-summary-value">${subtotal.toLocaleString()}</span>
        </div>
        <div className="cart-summary-item">
          <span className="cart-summary-label">IVA (19%):</span>
          <span className="cart-summary-value">${impuestos.toLocaleString()}</span>
        </div>
        <div className="cart-summary-item">
          <span className="cart-summary-label cart-summary-small">Productos:</span>
          <span className="cart-summary-value cart-summary-small">{items.length} items</span>
        </div>
      </div>
      
      <div className="cart-total-section">
        <div className="cart-total-row">
          <span className="cart-total-label">Total:</span>
          <span className="cart-total-value">${total.toLocaleString()}</span>
        </div>
      </div>

      <div className="cart-benefits">
        <div className="cart-benefit">
          <div className="benefit-dot green"></div>
          <span>Envío gratis en compras superiores a $500,000</span>
        </div>
        <div className="cart-benefit">
          <div className="benefit-dot blue"></div>
          <span>Garantía extendida incluida</span>
        </div>
        <div className="cart-benefit">
          <div className="benefit-dot purple"></div>
          <span>Soporte técnico 24/7</span>
        </div>
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
    <div className="container">
      <div className="page-header">
        <h2 className="page-title">Tu Carrito de Compras</h2>
        <p className="page-subtitle">
          Revisa tus productos antes de proceder al pago
        </p>
      </div>
      
      {cart.items.length === 0 ? (
        <div className="cart-empty">
          <ShoppingCart size={80} className="cart-empty-icon" />
          <h3 className="cart-empty-title">Tu carrito está vacío</h3>
          <p className="cart-empty-text">
            Agrega productos increíbles para comenzar tu compra
          </p>
          <div className="benefits">
            <div className="benefit-item">
              <div className="benefit-dot green"></div>
              Envío gratis
            </div>
            <div className="benefit-item">
              <div className="benefit-dot blue"></div>
              Garantía extendida
            </div>
            <div className="benefit-item">
              <div className="benefit-dot purple"></div>
              Soporte 24/7
            </div>
          </div>
        </div>
      ) : (
        <div className="cart-layout">
          <div>
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
            <button onClick={onCheckout} className="checkout-btn">
              🛒 Proceder a Pagar
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
    <div>
      <nav className="navbar">
        <div className="container">
          <div className="navbar-content">
            <div className="logo-section">
              <div className="logo-icon">
                <ShoppingCart size={24} />
              </div>
              <div>
                <h1 className="logo-text">TechStore</h1>
                <p className="logo-subtitle">Premium Tech Solutions</p>
              </div>
            </div>
            <div className="nav-buttons">
              {currentStep !== "confirmation" && (
                <>
                  <button
                    onClick={() => setCurrentStep("products")}
                    className={`nav-btn ${currentStep === "products" ? "active" : "inactive"}`}
                  >
                    🛍️ Productos
                  </button>
                  <button
                    onClick={() => setCurrentStep("cart")}
                    className={`nav-btn ${currentStep === "cart" ? "active" : "inactive"}`}
                  >
                    🛒 Mi Carrito
                    {cart.items.length > 0 && (
                      <span className="cart-badge">{cart.items.length}</span>
                    )}
                  </button>
                </>
              )}
            </div>
          </div>
        </div>
      </nav>

      {error && (
        <div className="error-message">
          <div className="error-icon">
            <AlertCircle size={24} />
          </div>
          <div className="error-content">
            <h3>¡Oops! Algo salió mal</h3>
            <p>{error}</p>
          </div>
        </div>
      )}

      <main className="main-content">
        {currentStep === "products" && <ProductList />}
        {currentStep === "cart" && <CartView onCheckout={handleCheckout} />}
        {currentStep === "checkout" && (
          <div className="container">
            <div className="page-header">
              <h2 className="page-title">Checkout - Completa tu Compra</h2>
            </div>
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

