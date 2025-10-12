using System;
using System.Collections.Generic;

namespace Arquitectura_DDD.Core.ValueObjects
{
    public sealed class MontoTotal : ValueObject
    {
        public decimal Subtotal { get; }
        public decimal Impuestos { get; }
        public decimal Descuentos { get; }
        public decimal Total { get; }

        private MontoTotal(decimal subtotal, decimal impuestos, decimal descuentos)
        {
            Subtotal = subtotal;
            Impuestos = impuestos;
            Descuentos = descuentos;
            Total = CalcularTotal(subtotal, impuestos, descuentos);
        }

        public static MontoTotal Create(decimal subtotal, decimal porcentajeImpuesto, decimal descuentos = 0)
        {
            if (subtotal <= 0) throw new ArgumentException("Subtotal debe ser mayor a cero", nameof(subtotal));
            if (porcentajeImpuesto < 0 || porcentajeImpuesto > 100)
                throw new ArgumentException("Impuesto debe estar entre 0-100", nameof(porcentajeImpuesto));
            if (descuentos < 0) throw new ArgumentException("Descuentos no pueden ser negativos", nameof(descuentos));
            if (descuentos > subtotal) throw new ArgumentException("Descuentos no pueden exceder subtotal", nameof(descuentos));

            var impuestos = subtotal * (porcentajeImpuesto / 100);
            return new MontoTotal(subtotal, impuestos, descuentos);
        }

        private static decimal CalcularTotal(decimal subtotal, decimal impuestos, decimal descuentos)
            => subtotal + impuestos - descuentos;

        public MontoTotal AplicarDescuentoAdicional(decimal descuentoAdicional)
        {
            if (descuentoAdicional <= 0) throw new ArgumentException("Descuento debe ser mayor a cero");
            var nuevoDescuento = Descuentos + descuentoAdicional;
            if (nuevoDescuento > Subtotal) throw new InvalidOperationException("Descuento total excede subtotal");

            return Create(Subtotal, (Impuestos / Subtotal) * 100, nuevoDescuento);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Subtotal;
            yield return Impuestos;
            yield return Descuentos;
            yield return Total;
        }

        public override string ToString() => $"Sub: {Subtotal:C}, Imp: {Impuestos:C}, Desc: {Descuentos:C}, Total: {Total:C}";

    }
}
