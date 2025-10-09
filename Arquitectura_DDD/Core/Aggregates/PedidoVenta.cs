using System.Collections.Generic;
using Core.Entities;
using System;

namespace Core.Aggregates
{
    public class PedidoVenta

    {
        
        public int Id { get; set; }
        public int PedidoId { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Total { get; set; }
        public string MetodoPago { get; set; }
        public List<Producto> Productos { get; set; }
        public Cliente Cliente { get; set; }
    }

    public class Producto
    {
        public int ProductoId { get; set; }
        public string Nombre { get; set; }
        public decimal Precio { get; set; }
        public int Cantidad { get; set; }
    }
}
