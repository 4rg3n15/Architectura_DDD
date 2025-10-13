using System;
using System.Collections.Generic;
using Arquitectura_DDD.Application.Common;
using Arquitectura_DDD.Application.DTOs;
using static Arquitectura_DDD.Application.DTOs.PedidoDto;

namespace Arquitectura_DDD.Application.Commands
{
    public class CrearPedidoCommand : IRequest<PedidoResponse>
    {
        public Guid ClienteId { get; init; }
        public string Calle { get; init; }
        public string Ciudad { get; init; }
        public string Departamento { get; init; }
        public string CodigoPostal { get; init; }
        public string Referencias { get; init; }
        public List<DetallePedidoItem> Detalles { get; init; }
    }
    public record DetallePedidoItem(
        string ProductoId,
        string NombreProducto,
        int Cantidad,
        decimal PrecioUnitario);
}
