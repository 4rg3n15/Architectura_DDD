using System;
using System.Collections.Generic;

namespace Arquitectura_DDD.Core.ValueObjects
{
    public sealed class MetodoPago : ValueObject
    {
        public string Tipo { get; }
        public string Proveedor { get; }
        public string NumeroReferencia { get; }

        public static readonly string Tarjeta = "tarjeta";
        public static readonly string Transferencia = "transferencia";
        public static readonly string Efectivo = "efectivo";

        private static readonly List<string> TiposValidos = new() { Tarjeta, Transferencia, Efectivo };

        private MetodoPago(string tipo, string proveedor, string numeroReferencia)
        {
            Tipo = tipo;
            Proveedor = proveedor;
            NumeroReferencia = numeroReferencia;
        }

        public static MetodoPago Create(string tipo, string proveedor, string numeroReferencia)
        {
            if (string.IsNullOrWhiteSpace(tipo) || !TiposValidos.Contains(tipo.ToLower()))
                throw new ArgumentException($"Tipo de pago no válido: {tipo}", nameof(tipo));
            if (string.IsNullOrWhiteSpace(proveedor))
                throw new ArgumentException("Proveedor no puede estar vacío", nameof(proveedor));
            if (string.IsNullOrWhiteSpace(numeroReferencia))
                throw new ArgumentException("Número de referencia no puede estar vacío", nameof(numeroReferencia));

            return new MetodoPago(tipo.ToLower(), proveedor.Trim(), numeroReferencia.Trim());
        }

        public bool RequiereVerificacion => Tipo == Tarjeta || Tipo == Transferencia;

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Tipo;
            yield return Proveedor;
            yield return NumeroReferencia;
        }
    }
}