using System;
using System.Collections.Generic;

namespace Arquitectura_DDD.Core.ValueObjects
{
    public sealed class EstadoPedido : ValueObject
    {
        public string CodigoEstado { get; }
        public string Descripcion { get; }
        public DateTime FechaActualizacion { get; }

        public static readonly string Pendiente = "pendiente";
        public static readonly string Pagado = "pagado";
        public static readonly string Enviado = "enviado";
        public static readonly string Entregado = "entregado";
        public static readonly string Cancelado = "cancelado";

        private static readonly Dictionary<string, string> DescripcionesEstados = new()
        {
            { Pendiente, "Pedido creado y pendiente de pago" },
            { Pagado, "Pedido pagado y en preparación" },
            { Enviado, "Pedido enviado al cliente" },
            { Entregado, "Pedido entregado al cliente" },
            { Cancelado, "Pedido cancelado" }
        };

        private static readonly Dictionary<string, List<string>> TransicionesValidas = new()
        {
            { Pendiente, new List<string> { Pagado, Cancelado } },
            { Pagado, new List<string> { Enviado, Cancelado } },
            { Enviado, new List<string> { Entregado } },
            { Entregado, new List<string> { } },
            { Cancelado, new List<string> { } }
        };

        private EstadoPedido(string codigoEstado, DateTime fechaActualizacion)
        {
            CodigoEstado = codigoEstado;
            Descripcion = DescripcionesEstados[codigoEstado];
            FechaActualizacion = fechaActualizacion;
        }

        public static EstadoPedido Create(string codigoEstado)
        {
            if (string.IsNullOrWhiteSpace(codigoEstado))
                throw new ArgumentException("Código de estado no puede estar vacío", nameof(codigoEstado));
            if (!DescripcionesEstados.ContainsKey(codigoEstado.ToLower()))
                throw new ArgumentException($"Estado '{codigoEstado}' no válido", nameof(codigoEstado));

            return new EstadoPedido(codigoEstado.ToLower(), DateTime.UtcNow);
        }

        public EstadoPedido TransicionarA(string nuevoEstado)
        {
            if (!TransicionesValidas[CodigoEstado].Contains(nuevoEstado))
                throw new InvalidOperationException($"Transición no permitida: {CodigoEstado} -> {nuevoEstado}");

            return Create(nuevoEstado);
        }

        public bool PuedeCancelar => CodigoEstado == Pendiente || CodigoEstado == Pagado;
        public bool EsFinal => CodigoEstado == Entregado || CodigoEstado == Cancelado;

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return CodigoEstado;
            yield return FechaActualizacion;
        }
    }
}
