using System;
using System.Threading.Tasks;
using Arquitectura_DDD.Core.Aggregates;
using Arquitectura_DDD.Core.Interfaces;
using Arquitectura_DDD.Core.Services;
using Arquitectura_DDD.Core.ValueObjects;

namespace Arquitectura_DDD.Application.UseCases
{
    public class CrearPedidoUseCase
    {
        private readonly ServicioGestionPedidos _servicioGestionPedidos;
        private readonly IServicioValidacionCredito _servicioValidacionCredito;
        private readonly IPedidoVentaRepository _pedidoRepository;
        private readonly IClienteRepository _clienteRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CrearPedidoUseCase(
            ServicioGestionPedidos servicioGestionPedidos,
            IServicioValidacionCredito servicioValidacionCredito,
            IPedidoVentaRepository pedidoRepository,
            IClienteRepository clienteRepository,
            IUnitOfWork unitOfWork)
        {
            _servicioGestionPedidos = servicioGestionPedidos ?? throw new ArgumentNullException(nameof(servicioGestionPedidos));
            _servicioValidacionCredito = servicioValidacionCredito ?? throw new ArgumentNullException(nameof(servicioValidacionCredito));
            _pedidoRepository = pedidoRepository ?? throw new ArgumentNullException(nameof(pedidoRepository));
            _clienteRepository = clienteRepository ?? throw new ArgumentNullException(nameof(clienteRepository));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<CrearPedidoResult> ExecuteAsync(CrearPedidoRequest request)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                // 1. Validar cliente existe
                var cliente = await _clienteRepository.GetByIdAsync(request.ClienteId);
                if (cliente == null)
                    throw new InvalidOperationException("Cliente no encontrado");

                // 2. Validar crédito del cliente
                var montoTotal = request.Detalles.Sum(d => d.Cantidad * d.PrecioUnitario);
                var creditoValido = await _servicioValidacionCredito.ValidarCapacidadPagoAsync(request.ClienteId, montoTotal);
                if (!creditoValido)
                    throw new InvalidOperationException("Crédito insuficiente para el pedido");

                // 3. Crear pedido usando servicio de dominio
                var numeroPedido = $"PED-{DateTime.UtcNow:yyyyMMdd-HHmmss}";
                var pedido = await _servicioGestionPedidos.CrearPedidoAsync(request.ClienteId, numeroPedido);

                // 4. Agregar detalles al pedido
                foreach (var detalleRequest in request.Detalles)
                {
                    await _servicioGestionPedidos.ValidarDisponibilidadProductosAsync(detalleRequest.ProductoId, detalleRequest.Cantidad);
                    pedido.AgregarDetalle(
                        detalleRequest.ProductoId,
                        detalleRequest.NombreProducto,
                        detalleRequest.Cantidad,
                        detalleRequest.PrecioUnitario
                    );
                }

                // 5. Persistir cambios
                await _pedidoRepository.UpdateAsync(pedido);
                await _unitOfWork.CommitTransactionAsync();

                return new CrearPedidoResult
                {
                    PedidoId = pedido.Id,
                    NumeroPedido = pedido.NumeroPedido,
                    MontoTotal = pedido.MontoTotal.Total,
                    Estado = pedido.Estado.Codigo.ToString()
                };
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }
    }

    public class CrearPedidoRequest
    {
        public Guid ClienteId { get; set; }
        public List<DetallePedidoRequest> Detalles { get; set; } = new();
    }

    public class DetallePedidoRequest
    {
        public Guid ProductoId { get; set; }
        public string NombreProducto { get; set; } = string.Empty;
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
    }

    public class CrearPedidoResult
    {
        public Guid PedidoId { get; set; }
        public string NumeroPedido { get; set; } = string.Empty;
        public decimal MontoTotal { get; set; }
        public string Estado { get; set; } = string.Empty;
    }
}
