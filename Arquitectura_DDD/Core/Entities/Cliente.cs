using System;
using System.Collections.Generic;

namespace Core.Entities
{
    public class Cliente
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public DateTime FechaRegistro { get; set; }
        public bool Activo { get; set; }

        // Navegación para Pedidos
        public ICollection<Pedido> Pedidos { get; set; }
    }
}