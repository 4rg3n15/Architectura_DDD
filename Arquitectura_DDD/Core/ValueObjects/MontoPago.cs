using System;
using System.Collections.Generic;

namespace Arquitectura_DDD.Core.ValueObjects
{
    public sealed class MontoPago : ValueObject
    {
        public enum TipoPago { Tarjeta, Transferencia, Efectivo }

        public TipoPago Tipo { get; }
        public string Proveedor { get; }
        public string NumeroReferencia { get; }

        public MontoPago(TipoPago tipo, string proveedor, string numeroReferencia)
        {
            if (string.IsNullOrWhiteSpace(proveedor))
                throw new ArgumentException("El proveedor no puede estar vacío", nameof(proveedor));
            if (string.IsNullOrWhiteSpace(numeroReferencia))
                throw new ArgumentException("El número de referencia no puede estar vacío", nameof(numeroReferencia));

            Tipo = tipo;
            Proveedor = proveedor.Trim();
            NumeroReferencia = numeroReferencia.Trim();
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Tipo;
            yield return Proveedor;
            yield return NumeroReferencia;
        }
    }
}