using System;
using System.Collections.Generic;
using Arquitectura_DDD.Core.Common;

namespace Arquitectura_DDD.Core.ValueObjects
{
    public sealed class CanalNotificacion : ValueObject
    {
        public enum TipoCanal { Email, SMS, WhatsApp, Push }

        public TipoCanal Tipo { get; }
        public string DireccionDestino { get; }

        public CanalNotificacion(TipoCanal tipo, string direccionDestino)
        {
            if (string.IsNullOrWhiteSpace(direccionDestino))
                throw new ArgumentException("La dirección de destino no puede estar vacía", nameof(direccionDestino));

            Tipo = tipo;
            DireccionDestino = direccionDestino.Trim();
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Tipo;
            yield return DireccionDestino;
        }
    }
}
