using System;
using Arquitectura_DDD.Core.Common;
using Arquitectura_DDD.Core.ValueObjects;

namespace Arquitectura_DDD.Core.Entities
{
    public class Pedido : Entity
    {
        public DateTime Fecha { get; private set; }
        public MontoTotal Total { get; private set; }
        public Guid ClienteId { get; private set; }
        
        // Constructor privado para EF
        private Pedido() { }

        public Pedido(DateTime fecha, MontoTotal total, Guid clienteId)
        {
            if (fecha == default)
                throw new ArgumentException("La fecha no puede ser vacía", nameof(fecha));
            if (total == null)
                throw new ArgumentNullException(nameof(total));
            if (clienteId == Guid.Empty)
                throw new ArgumentException("El ID del cliente no puede estar vacío", nameof(clienteId));

            Fecha = fecha;
            Total = total;
            ClienteId = clienteId;
        }

        public void ActualizarTotal(MontoTotal nuevoTotal)
        {
            Total = nuevoTotal ?? throw new ArgumentNullException(nameof(nuevoTotal));
        }
    }
}