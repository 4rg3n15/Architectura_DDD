using System;
using System.Collections.Generic;
using Arquitectura_DDD.Core.Common;

namespace Arquitectura_DDD.Core.ValueObjects
{
    public sealed class MontoTotal : ValueObject
    {
        public decimal Subtotal { get; }
        public decimal Impuestos { get; }
        public decimal Descuentos { get; }
        public decimal Total { get; }

        public MontoTotal(decimal subtotal, decimal impuestos, decimal descuentos)
        {
            if (subtotal < 0)
                throw new ArgumentException("El subtotal no puede ser negativo", nameof(subtotal));
            if (impuestos < 0)
                throw new ArgumentException("Los impuestos no pueden ser negativos", nameof(impuestos));
            if (descuentos < 0)
                throw new ArgumentException("Los descuentos no pueden ser negativos", nameof(descuentos));

            Subtotal = subtotal;
            Impuestos = impuestos;
            Descuentos = descuentos;
            Total = subtotal + impuestos - descuentos;

            if (Total < 0)
                throw new ArgumentException("El total no puede ser negativo");
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Subtotal;
            yield return Impuestos;
            yield return Descuentos;
            yield return Total;
        }
    }
}