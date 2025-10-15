import React from "react";

export default function QuantityInput({ value, min = 1, max = 999, onChange }) {
  return (
    <div className="row" style={{ gap: ".25rem" }}>
      <button
        className="btn ghost"
        onClick={() => onChange(Math.max(min, value - 1))}
      >
        -
      </button>
      <input
        className="input"
        style={{ width: 64, textAlign: "center" }}
        type="number"
        value={value}
        min={min}
        max={max}
        onChange={(e) =>
          onChange(Math.max(min, Math.min(max, Number(e.target.value) || min)))
        }
      />
      <button
        className="btn ghost"
        onClick={() => onChange(Math.min(max, value + 1))}
      >
        +
      </button>
    </div>
  );
}
