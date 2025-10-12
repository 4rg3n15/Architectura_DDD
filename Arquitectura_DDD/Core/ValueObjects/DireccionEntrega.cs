using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Arquitectura_DDD.Core.ValueObjects
{
    public sealed class DireccionEntrega : ValueObject
    {
        public string Calle { get; }
        public string Ciudad { get; }
        public string Departamento { get; }
        public string CodigoPostal { get; }
        public string Referencias { get; }

        private DireccionEntrega(string calle, string ciudad, string departamento, string codigoPostal, string referencias)
        {
            Calle = calle;
            Ciudad = ciudad;
            Departamento = departamento;
            CodigoPostal = codigoPostal;
            Referencias = referencias;
        }

        public static DireccionEntrega Create(string calle, string ciudad, string departamento, string codigoPostal, string referencias = "")
        {
            if (string.IsNullOrWhiteSpace(calle))
                throw new ArgumentException("La calle no puede estar vacía", nameof(calle));
            if (string.IsNullOrWhiteSpace(ciudad))
                throw new ArgumentException("La ciudad no puede estar vacía", nameof(ciudad));
            if (string.IsNullOrWhiteSpace(departamento))
                throw new ArgumentException("El departamento no puede estar vacío", nameof(departamento));
            if (string.IsNullOrWhiteSpace(codigoPostal))
                throw new ArgumentException("El código postal no puede estar vacío", nameof(codigoPostal));
            if (!Regex.IsMatch(codigoPostal, @"^\d{4,6}$"))
                throw new ArgumentException("Formato de código postal inválido", nameof(codigoPostal));

            return new DireccionEntrega(calle.Trim(), ciudad.Trim(), departamento.Trim(), codigoPostal.Trim(), referencias?.Trim() ?? "");
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Calle;
            yield return Ciudad;
            yield return Departamento;
            yield return CodigoPostal;
            yield return Referencias;
        }

        public override string ToString() => $"{Calle}, {Ciudad}, {Departamento} - {CodigoPostal}";
    }
}
