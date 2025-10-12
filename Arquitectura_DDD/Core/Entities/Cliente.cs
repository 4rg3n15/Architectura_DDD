using System;
using System.Collections.Generic;
using System.Linq;
using Arquitectura_DDD.Core.Common;
using Arquitectura_DDD.Core.ValueObjects;
using Arquitectura_DDD.Core.Events;
using Arquitectura_DDD.Core.Aggregates;

namespace Arquitectura_DDD.Core.Entities
{
    public class Cliente : AggregateRoot
    {
        public Guid Id { get; private set; }
        public string Nombre { get; private set; }
        public string Email { get; private set; }
        public string Telefono { get; private set; }
        public DireccionEntrega DireccionEntrega { get; private set; }

        private readonly List<Guid> _pedidosIds;
        public IReadOnlyCollection<Guid> PedidosIds => _pedidosIds.AsReadOnly();

        // Constructor privado para EF
        private Cliente() { }

        public Cliente(Guid id, string nombre, string email, string telefono, DireccionEntrega direccionEntrega)
        {
            Id = id;
            SetNombre(nombre);
            SetEmail(email);
            SetTelefono(telefono);
            DireccionEntrega = direccionEntrega ?? throw new ArgumentNullException(nameof(direccionEntrega));
            _pedidosIds = new List<Guid>();
        }

        // Comportamientos ricos
        public void SetNombre(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("Nombre no puede estar vacío", nameof(nombre));
            if (nombre.Length < 2 || nombre.Length > 100)
                throw new ArgumentException("Nombre debe tener entre 2-100 caracteres", nameof(nombre));

            Nombre = nombre.Trim();
        }

        public void SetEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email no puede estar vacío", nameof(email));
            if (!email.Contains("@"))
                throw new ArgumentException("Formato de email inválido", nameof(email));

            Email = email.Trim().ToLower();
        }

        public void SetTelefono(string telefono)
        {
            if (string.IsNullOrWhiteSpace(telefono))
                throw new ArgumentException("Teléfono no puede estar vacío", nameof(telefono));

            Telefono = telefono.Trim();
        }

        public void ActualizarDireccion(DireccionEntrega nuevaDireccion)
        {
            DireccionEntrega = nuevaDireccion ?? throw new ArgumentNullException(nameof(nuevaDireccion));
        }

        public void AgregarPedido(Guid pedidoId)
        {
            if (pedidoId == Guid.Empty)
                throw new ArgumentException("ID de pedido no válido", nameof(pedidoId));

            if (!_pedidosIds.Contains(pedidoId))
                _pedidosIds.Add(pedidoId);
        }

        public bool TienePedidosActivos() => _pedidosIds.Any();
    }
}