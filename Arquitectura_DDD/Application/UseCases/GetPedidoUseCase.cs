using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arquitectura_DDD.Core.Aggregates;
using Arquitectura_DDD.Core.Interfaces;

namespace Arquitectura_DDD.Application.UseCases
{
    public class GetPedidoUseCase
    {
        private readonly IPedidoVentaRepository _pedidoRepository;

        public GetPedidoUseCase(IPedidoVentaRepository pedidoRepository)
        {
            _pedidoRepository = pedidoRepository ?? throw new ArgumentNullException(nameof(pedidoRepository));
        }

        public async Task<GetPedidoResult?> ExecuteAsync(Guid id)
        {
            var pedido = await _pedidoRepository.GetByIdAsync(id);
            if (pedido == null)
                return null;

            return MapToResult(pedido);
        }

        private static GetPedidoResult MapToResult(PedidoVenta pedido)
        {
            return new GetPedidoResult
            {
                Id = pedido.Id,
                NumeroPedido = pedido.NumeroPedido,
                ClienteId = pedido.ClienteId,
                FechaCreacion = pedido.FechaCreacion,
                Estado = pedido.Estado.Codigo.ToString(),
                MetodoPago = pedido.MetodoPago is null ? null : new MetodoPagoResult
                {
                    Tipo = pedido.MetodoPago.Tipo.ToString(),
                    Proveedor = pedido.MetodoPago.Proveedor,
                    NumeroReferencia = pedido.MetodoPago.NumeroReferencia
                },
                MontoTotal = new MontoTotalResult
                {
                    Subtotal = pedido.MontoTotal.Subtotal,
                    Impuestos = pedido.MontoTotal.Impuestos,
                    Descuentos = pedido.MontoTotal.Descuentos,
                    Total = pedido.MontoTotal.Total
                },
                Detalles = pedido.Detalles.Select(d => new DetallePedidoResult
                {
                    ProductoId = d.ProductoId,
                    NombreProducto = d.NombreProducto,
                    Cantidad = d.Cantidad,
                    PrecioUnitario = d.PrecioUnitario,
                    Subtotal = d.Subtotal
                }).ToList()
            };
        }
    }

    public class GetPedidoResult
    {
        public Guid Id { get; set; }
        public string NumeroPedido { get; set; } = string.Empty;
        public Guid ClienteId { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string Estado { get; set; } = string.Empty;
        public MetodoPagoResult? MetodoPago { get; set; }
        public MontoTotalResult MontoTotal { get; set; } = null!;
        public List<DetallePedidoResult> Detalles { get; set; } = new();
    }

    public class MontoTotalResult
    {
        public decimal Subtotal { get; set; }
        public decimal Impuestos { get; set; }
        public decimal Descuentos { get; set; }
        public decimal Total { get; set; }
    }

    public class MetodoPagoResult
    {
        public string Tipo { get; set; } = string.Empty;
        public string Proveedor { get; set; } = string.Empty;
        public string NumeroReferencia { get; set; } = string.Empty;
    }

    public class DetallePedidoResult
    {
        public Guid ProductoId { get; set; }
        public string NombreProducto { get; set; } = string.Empty;
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal { get; set; }
    }
}


