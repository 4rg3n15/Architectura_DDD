using System;

namespace Core.Entities
{
    public class Pedido
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Total { get; set; }
        // Almacena la lista de productos del pedido como JSON
        public string ProductosJson { get; set; }
        // Foreign Key para Cliente
        public int ClienteId { get; set; }
        // Navegación para Cliente
        public Cliente Cliente { get; set; }
    }
}