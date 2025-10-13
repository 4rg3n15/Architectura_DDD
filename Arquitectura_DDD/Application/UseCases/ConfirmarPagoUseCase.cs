using System;
using System.Threading.Tasks;
using Arquitectura_DDD.Core.Aggregates;
using Arquitectura_DDD.Core.Interfaces;
using Arquitectura_DDD.Core.Services;
using Arquitectura_DDD.Core.ValueObjects;

namespace Arquitectura_DDD.Application.UseCases
{
    public class ConfirmarPagoUseCase
    {
        private readonly ServicioProcesamientoVentas _servicioProcesamientoVentas;
        private readonly IPedidoVentaRepository _pedidoRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ConfirmarPagoUseCase(
            ServicioProcesamientoVentas servicioProcesamientoVentas,
            IPedidoVentaRepository pedidoRepository,
            IUnitOfWork unitOfWork)
        {
            _servicioProcesamientoVentas = servicioProcesamientoVentas ?? throw new ArgumentNullException(nameof(servicioProcesamientoVentas));
            _pedidoRepository = pedidoRepository ?? throw new ArgumentNullException(nameof(pedidoRepository));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<ConfirmarPagoResult> ExecuteAsync(ConfirmarPagoRequest request)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                // 1. Verificar que el pedido exista
                var existente = await _pedidoRepository.GetByIdAsync(request.PedidoId);
                if (existente == null)
                    throw new InvalidOperationException("Pedido no encontrado");

                // 2. Crear método de pago
                var metodoPago = new MetodoPago(
                    request.TipoPago,
                    request.Proveedor,
                    request.NumeroReferencia
                );

                // 3. Confirmar pago usando servicio de dominio (este método persiste el cambio)
                await _servicioProcesamientoVentas.ConfirmarVentaAsync(request.PedidoId, metodoPago);

                // 4. Releer el pedido actualizado para evitar sobrescribir cambios del dominio
                var pedidoActualizado = await _pedidoRepository.GetByIdAsync(request.PedidoId);
                await _unitOfWork.CommitTransactionAsync();

                return new ConfirmarPagoResult
                {
                    PedidoId = pedidoActualizado.Id,
                    NumeroPedido = pedidoActualizado.NumeroPedido,
                    Estado = pedidoActualizado.Estado.Codigo.ToString(),
                    MontoPagado = pedidoActualizado.MontoTotal.Total
                };
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }
    }

    public class ConfirmarPagoRequest
    {
        public Guid PedidoId { get; set; }
        public MetodoPago.TipoPago TipoPago { get; set; }
        public string Proveedor { get; set; } = string.Empty;
        public string NumeroReferencia { get; set; } = string.Empty;
    }

    public class ConfirmarPagoResult
    {
        public Guid PedidoId { get; set; }
        public string NumeroPedido { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public decimal MontoPagado { get; set; }
    }
}
