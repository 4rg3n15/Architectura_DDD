using System;
using System.Collections.Generic;
using Arquitectura_DDD.Core.Common;

namespace Arquitectura_DDD.Core.ValueObjects
{
    public sealed class DireccionEntrega : ValueObject
    {
        public string Calle { get; }
        public string Ciudad { get; }
        public string Departamento { get; }
        public string CodigoPostal { get; }
        public string Referencias { get; }

        public DireccionEntrega(string calle, string ciudad, string departamento, string codigoPostal, string referencias = "")
        {
            if (string.IsNullOrWhiteSpace(calle))
                throw new ArgumentException("La calle no puede estar vacía", nameof(calle));
            if (string.IsNullOrWhiteSpace(ciudad))
                throw new ArgumentException("La ciudad no puede estar vacía", nameof(ciudad));
            if (string.IsNullOrWhiteSpace(departamento))
                throw new ArgumentException("El departamento no puede estar vacío", nameof(departamento));
            if (string.IsNullOrWhiteSpace(codigoPostal))
                throw new ArgumentException("El código postal no puede estar vacío", nameof(codigoPostal));

            Calle = calle.Trim();
            Ciudad = ciudad.Trim();
            Departamento = departamento.Trim();
            CodigoPostal = codigoPostal.Trim();
            Referencias = referencias?.Trim() ?? "";
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Calle;
            yield return Ciudad;
            yield return Departamento;
            yield return CodigoPostal;
            yield return Referencias;
        }
    }
}
