using System;
using System.Collections.Generic;
using Arquitectura_DDD.Core.Common;

namespace Arquitectura_DDD.Core.ValueObjects
{
    public sealed class DetallePedido : ValueObject
    {
        public Guid ProductoId { get; }
        public string NombreProducto { get; }
        public int Cantidad { get; }
        public decimal PrecioUnitario { get; }
        public decimal Subtotal => Cantidad * PrecioUnitario;

        public DetallePedido(Guid productoId, string nombreProducto, int cantidad, decimal precioUnitario)
        {
            if (productoId == Guid.Empty)
                throw new ArgumentException("El ID del producto no puede estar vacío", nameof(productoId));
            if (string.IsNullOrWhiteSpace(nombreProducto))
                throw new ArgumentException("El nombre del producto no puede estar vacío", nameof(nombreProducto));
            if (cantidad <= 0)
                throw new ArgumentException("La cantidad debe ser mayor a cero", nameof(cantidad));
            if (precioUnitario < 0)
                throw new ArgumentException("El precio unitario no puede ser negativo", nameof(precioUnitario));

            ProductoId = productoId;
            NombreProducto = nombreProducto.Trim();
            Cantidad = cantidad;
            PrecioUnitario = precioUnitario;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return ProductoId;
            yield return NombreProducto;
            yield return Cantidad;
            yield return PrecioUnitario;
        }
    }
}
