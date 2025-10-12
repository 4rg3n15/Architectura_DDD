using System;
using System.Collections.Generic;

namespace Arquitectura_DDD.Core.ValueObjects
{
    public sealed class DetallePedido : ValueObject
    {
        public string ProductoId { get; }
        public string NombreProducto { get; }
        public int Cantidad { get; }
        public decimal PrecioUnitario { get; }
        public decimal Subtotal => Cantidad * PrecioUnitario;

        private DetallePedido(string productoId, string nombreProducto, int cantidad, decimal precioUnitario)
        {
            ProductoId = productoId;
            NombreProducto = nombreProducto;
            Cantidad = cantidad;
            PrecioUnitario = precioUnitario;
        }

        public static DetallePedido Create(string productoId, string nombreProducto, int cantidad, decimal precioUnitario)
        {
            if (string.IsNullOrWhiteSpace(productoId))
                throw new ArgumentException("ID de producto no puede estar vacío", nameof(productoId));
            if (string.IsNullOrWhiteSpace(nombreProducto))
                throw new ArgumentException("Nombre de producto no puede estar vacío", nameof(nombreProducto));
            if (cantidad <= 0)
                throw new ArgumentException("Cantidad debe ser mayor a cero", nameof(cantidad));
            if (precioUnitario <= 0)
                throw new ArgumentException("Precio unitario debe ser mayor a cero", nameof(precioUnitario));

            return new DetallePedido(productoId.Trim(), nombreProducto.Trim(), cantidad, precioUnitario);
        }

        public DetallePedido ActualizarCantidad(int nuevaCantidad)
        {
            if (nuevaCantidad <= 0)
                throw new ArgumentException("Cantidad debe ser mayor a cero", nameof(nuevaCantidad));

            return Create(ProductoId, NombreProducto, nuevaCantidad, PrecioUnitario);
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
