using System;
using System.Collections.Generic;

namespace Arquitectura_DDD.Core.ValueObjects
{
    public sealed class DatosFactura : ValueObject
    {
        public string NumeroFactura { get; }
        public DateTime FechaEmision { get; }
        public string NitCliente { get; }
        public decimal ValorTotal { get; }

        private DatosFactura(string numeroFactura, DateTime fechaEmision, string nitCliente, decimal valorTotal)
        {
            NumeroFactura = numeroFactura;
            FechaEmision = fechaEmision;
            NitCliente = nitCliente;
            ValorTotal = valorTotal;
        }

        public static DatosFactura Create(string numeroFactura, string nitCliente, decimal valorTotal)
        {
            if (string.IsNullOrWhiteSpace(numeroFactura))
                throw new ArgumentException("Número de factura no puede estar vacío", nameof(numeroFactura));
            if (string.IsNullOrWhiteSpace(nitCliente))
                throw new ArgumentException("NIT del cliente no puede estar vacío", nameof(nitCliente));
            if (valorTotal <= 0)
                throw new ArgumentException("Valor total debe ser mayor a cero", nameof(valorTotal));

            return new DatosFactura(numeroFactura.Trim(), DateTime.UtcNow, nitCliente.Trim(), valorTotal);
        }

        public bool EsFacturaElectronica => NumeroFactura.StartsWith("FE");

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return NumeroFactura;
            yield return FechaEmision;
            yield return NitCliente;
            yield return ValorTotal;
        }
    }
}
