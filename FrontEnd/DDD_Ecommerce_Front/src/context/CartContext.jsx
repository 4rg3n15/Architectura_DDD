import React, { createContext, useContext, useReducer, useMemo } from "react";

const CartContext = createContext(null);

const initial = { items: [] }; // [{ productoId, nombreProducto, precioUnitario, cantidad }]

function cartReducer(state, action) {
  switch (action.type) {
    case "ADD_ITEM": {
      const i = state.items.find(
        (x) => x.productoId === action.payload.productoId
      );
      if (i) {
        return {
          ...state,
          items: state.items.map((x) =>
            x.productoId === action.payload.productoId
              ? { ...x, cantidad: x.cantidad + action.payload.cantidad }
              : x
          ),
        };
      }
      return { ...state, items: [...state.items, action.payload] };
    }
    case "UPDATE_QUANTITY":
      return {
        ...state,
        items: state.items
          .map((x) =>
            x.productoId === action.payload.productoId
              ? { ...x, cantidad: action.payload.cantidad }
              : x
          )
          .filter((x) => x.cantidad > 0),
      };
    case "REMOVE_ITEM":
      return {
        ...state,
        items: state.items.filter((x) => x.productoId !== action.payload),
      };
    case "CLEAR_CART":
      return { ...state, items: [] };
    default:
      return state;
  }
}

export function CartProvider({ children }) {
  const [cart, dispatch] = useReducer(cartReducer, initial);
  const subtotal = useMemo(
    () => cart.items.reduce((t, it) => t + it.cantidad * it.precioUnitario, 0),
    [cart]
  );
  const value = { cart, dispatch, subtotal };
  return <CartContext.Provider value={value}>{children}</CartContext.Provider>;
}

export const useCart = () => {
  const ctx = useContext(CartContext);
  if (!ctx) throw new Error("useCart must be used within CartProvider");
  return ctx;
};
