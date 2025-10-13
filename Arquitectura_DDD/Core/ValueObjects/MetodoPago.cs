using System;
using System.Collections.Generic;
using Arquitectura_DDD.Core.Common;

namespace Arquitectura_DDD.Core.ValueObjects
{
    public sealed class MetodoPago : ValueObject
    {
        public enum TipoPago { Tarjeta, Transferencia, Efectivo }

        public TipoPago Tipo { get; }
        public string Proveedor { get; }
        public string NumeroReferencia { get; }

        public MetodoPago(TipoPago tipo, string proveedor, string numeroReferencia)
        {
            if (string.IsNullOrWhiteSpace(proveedor))
                throw new ArgumentException("El proveedor no puede estar vacío", nameof(proveedor));
            if (string.IsNullOrWhiteSpace(numeroReferencia))
                throw new ArgumentException("El número de referencia no puede estar vacío", nameof(numeroReferencia));

            Tipo = tipo;
            Proveedor = proveedor.Trim();
            NumeroReferencia = numeroReferencia.Trim();
        }

        // Constructor privado para MongoDB
        private MetodoPago() { }

        public bool RequiereVerificacion => Tipo == TipoPago.Tarjeta || Tipo == TipoPago.Transferencia;

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Tipo;
            yield return Proveedor;
            yield return NumeroReferencia;
        }
    }
}