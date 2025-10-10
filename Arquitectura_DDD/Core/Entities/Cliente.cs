using System;
using System.Collections.Generic;
using System.Linq;
using Arquitectura_DDD.Core.Common;
using Arquitectura_DDD.Core.ValueObjects;
using Arquitectura_DDD.Core.Events;

namespace Arquitectura_DDD.Core.Entities
{
    public class Cliente : Entity, IAggregateRoot
    {
        public string Nombre { get; private set; } = string.Empty;
        public string Email { get; private set; } = string.Empty;
        public string Telefono { get; private set; } = string.Empty;
        public DireccionEntrega DireccionEntrega { get; private set; } = null!;
        public decimal LimiteCredito { get; private set; }
        public bool Activo { get; private set; }

        // No direct collection of PedidoVenta - this violates DDD aggregate boundaries
        // PedidoVenta is its own aggregate root

        // Constructor privado para EF
        private Cliente() { }

        public Cliente(string nombre, string email, string telefono, DireccionEntrega direccionEntrega, decimal limiteCredito)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("El nombre no puede estar vacío");
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("El email no puede estar vacío");

            Nombre = nombre.Trim();
            Email = email.Trim();
            Telefono = telefono?.Trim() ?? "";
            DireccionEntrega = direccionEntrega ?? throw new ArgumentNullException(nameof(direccionEntrega));
            LimiteCredito = limiteCredito >= 0 ? limiteCredito : throw new ArgumentException("El límite de crédito no puede ser negativo");
            Activo = true;

            AddDomainEvent(new ClienteCreado(Id, Nombre, Email));
        }

        // Comportamientos ricos
        public void ActualizarInformacion(string nombre, string email, string telefono, DireccionEntrega direccion)
        {
            if (!Activo)
                throw new InvalidOperationException("No se puede actualizar un cliente inactivo");

            Nombre = !string.IsNullOrWhiteSpace(nombre) ? nombre.Trim() : Nombre;
            Email = !string.IsNullOrWhiteSpace(email) ? email.Trim() : Email;
            Telefono = telefono?.Trim() ?? Telefono;
            DireccionEntrega = direccion ?? DireccionEntrega;

            AddDomainEvent(new ClienteActualizado(Id));
        }

        public void ActualizarLimiteCredito(decimal nuevoLimite)
        {
            if (nuevoLimite < 0)
                throw new ArgumentException("El límite de crédito no puede ser negativo");

            LimiteCredito = nuevoLimite;
        }

        public void Desactivar()
        {
            if (!Activo) return;

            Activo = false;
            AddDomainEvent(new ClienteDesactivado(Id));
        }

        public void Reactivar()
        {
            if (Activo) return;

            Activo = true;
            AddDomainEvent(new ClienteReactivado(Id));
        }

        public bool TieneCreditoDisponible(decimal montoPedido)
        {
            // This method would need to query the repository for pending orders
            // For now, we'll assume the client has credit available if the order amount
            // is within the credit limit
            return montoPedido <= LimiteCredito;
        }
    }
}