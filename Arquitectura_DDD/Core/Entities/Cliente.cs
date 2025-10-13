using System;
using System.Collections.Generic;
using System.Linq;
using Arquitectura_DDD.Core.Common;
using Arquitectura_DDD.Core.ValueObjects;
using Arquitectura_DDD.Core.Events;
using MongoDB.Bson.Serialization.Attributes;

namespace Arquitectura_DDD.Core.Entities
{
    [BsonIgnoreExtraElements]
    public class Cliente : Entity, IAggregateRoot
    {
        public string Nombre { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public DireccionEntrega DireccionEntrega { get; set; } = null!;
        [MongoDB.Bson.Serialization.Attributes.BsonElement("limiteCredito")]
        public decimal LimiteCredito { get; set; }
        [MongoDB.Bson.Serialization.Attributes.BsonElement("activo")]
        public bool Activo { get; set; }

        // No direct collection of PedidoVenta - this violates DDD aggregate boundaries
        // PedidoVenta is its own aggregate root

        // Constructor privado para MongoDB
        private Cliente() 
        {
            Nombre = string.Empty;
            Email = string.Empty;
            Telefono = string.Empty;
            Activo = true; // Valor por defecto
            // DireccionEntrega se inicializará desde la base de datos
        }

        public Cliente(string nombre, string email, string telefono, DireccionEntrega direccionEntrega, decimal limiteCredito)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("El nombre no puede estar vacío");
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("El email no puede estar vacío");

            Id = Guid.NewGuid();
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
            // Log temporal para diagnóstico
            System.Diagnostics.Debug.WriteLine($"DEBUG Cliente.TieneCreditoDisponible: montoPedido={montoPedido:C}, LimiteCredito={LimiteCredito:C}");
            bool resultado = montoPedido <= LimiteCredito;
            System.Diagnostics.Debug.WriteLine($"DEBUG Cliente.TieneCreditoDisponible: resultado={resultado}");
            return resultado;
        }
    }
}