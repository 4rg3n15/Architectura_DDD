using System;
using System.Collections.Generic;
using Arquitectura_DDD.Core.Common;

namespace Arquitectura_DDD.Core.ValueObjects
{
    public sealed class DatosFactura : ValueObject
    {
        public string NumeroFactura { get; }
        public DateTime FechaEmision { get; }
        public string NITCliente { get; }
        public decimal ValorTotal { get; }

        public DatosFactura(string numeroFactura, DateTime fechaEmision, string nitCliente, decimal valorTotal)
        {
            if (string.IsNullOrWhiteSpace(numeroFactura))
                throw new ArgumentException("El número de factura no puede estar vacío", nameof(numeroFactura));
            if (string.IsNullOrWhiteSpace(nitCliente))
                throw new ArgumentException("El NIT del cliente no puede estar vacío", nameof(nitCliente));
            if (valorTotal <= 0)
                throw new ArgumentException("El valor total debe ser mayor a cero", nameof(valorTotal));

            NumeroFactura = numeroFactura.Trim();
            FechaEmision = fechaEmision;
            NITCliente = nitCliente.Trim();
            ValorTotal = valorTotal;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return NumeroFactura;
            yield return FechaEmision;
            yield return NITCliente;
            yield return ValorTotal;
        }
    }
}
