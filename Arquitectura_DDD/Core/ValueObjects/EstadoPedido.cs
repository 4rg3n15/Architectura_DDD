using System;
using System.Collections.Generic;
using Arquitectura_DDD.Core.Common;

namespace Arquitectura_DDD.Core.ValueObjects
{
    public sealed class EstadoPedido : ValueObject
    {
        public enum CodigoEstado { Pendiente, Pagado, Enviado, Entregado, Cancelado }

        public CodigoEstado Codigo { get; }
        public DateTime FechaActualizacion { get; }

        public EstadoPedido(CodigoEstado codigo, DateTime fechaActualizacion)
        {
            Codigo = codigo;
            FechaActualizacion = fechaActualizacion;
        }

        // Constructor privado para MongoDB
        private EstadoPedido() { }

        public static EstadoPedido Pendiente() => new(CodigoEstado.Pendiente, DateTime.UtcNow);
        public static EstadoPedido Pagado() => new(CodigoEstado.Pagado, DateTime.UtcNow);
        public static EstadoPedido Enviado() => new(CodigoEstado.Enviado, DateTime.UtcNow);
        public static EstadoPedido Entregado() => new(CodigoEstado.Entregado, DateTime.UtcNow);
        public static EstadoPedido Cancelado() => new(CodigoEstado.Cancelado, DateTime.UtcNow);

        public bool PuedeCancelar => Codigo == CodigoEstado.Pendiente || Codigo == CodigoEstado.Pagado;
        public bool EsFinal => Codigo == CodigoEstado.Entregado || Codigo == CodigoEstado.Cancelado;

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Codigo;
            yield return FechaActualizacion;
        }
    }
}