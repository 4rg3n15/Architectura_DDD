namespace Arquitectura_DDD.Core.Aggregates
{
    public class DetallePedido : Entity
    {
        public Guid ProductoId { get; private set; }
        public string NombreProducto { get; private set; }
        public int Cantidad { get; private set; }
        public decimal PrecioUnitario { get; private set; }
        public decimal Subtotal => PrecioUnitario * Cantidad;

        // Constructor privado para EF
        private DetallePedido() { }

        public DetallePedido(Guid productoId, string nombreProducto, int cantidad, decimal precioUnitario)
        {
            if (productoId == Guid.Empty)
                throw new DetallePedidoException("El ID del producto no puede estar vacío");
            if (string.IsNullOrWhiteSpace(nombreProducto))
                throw new DetallePedidoException("El nombre del producto no puede estar vacío");
            if (cantidad <= 0)
                throw new DetallePedidoException("La cantidad debe ser mayor a cero");
            if (precioUnitario < 0)
                throw new DetallePedidoException("El precio unitario no puede ser negativo");

            ProductoId = productoId;
            NombreProducto = nombreProducto.Trim();
            Cantidad = cantidad;
            PrecioUnitario = precioUnitario;
        }

        public void ActualizarCantidad(int nuevaCantidad)
        {
            if (nuevaCantidad <= 0)
                throw new DetallePedidoException("La cantidad debe ser mayor a cero");

            Cantidad = nuevaCantidad;
        }

        public void ActualizarPrecio(decimal nuevoPrecio)
        {
            if (nuevoPrecio < 0)
                throw new DetallePedidoException("El precio unitario no puede ser negativo");

            PrecioUnitario = nuevoPrecio;
        }
    }
}
