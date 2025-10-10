using System;
using System.Threading.Tasks;
using Arquitectura_DDD.Core.Aggregates;
using Arquitectura_DDD.Core.Interfaces;
using Arquitectura_DDD.Core.Services;

namespace Arquitectura_DDD.Application.UseCases
{
    public class CancelarPedidoUseCase
    {
        private readonly IPedidoVentaRepository _pedidoRepository;
        private readonly IServicioNotificacionClientes _servicioNotificacion;
        private readonly IClienteRepository _clienteRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CancelarPedidoUseCase(
            IPedidoVentaRepository pedidoRepository,
            IServicioNotificacionClientes servicioNotificacion,
            IClienteRepository clienteRepository,
            IUnitOfWork unitOfWork)
        {
            _pedidoRepository = pedidoRepository ?? throw new ArgumentNullException(nameof(pedidoRepository));
            _servicioNotificacion = servicioNotificacion ?? throw new ArgumentNullException(nameof(servicioNotificacion));
            _clienteRepository = clienteRepository ?? throw new ArgumentNullException(nameof(clienteRepository));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<CancelarPedidoResult> ExecuteAsync(CancelarPedidoRequest request)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                // 1. Obtener pedido
                var pedido = await _pedidoRepository.GetByIdAsync(request.PedidoId);
                if (pedido == null)
                    throw new InvalidOperationException("Pedido no encontrado");

                // 2. Cancelar pedido (lógica en el agregado)
                pedido.Cancelar(request.Motivo);

                // 3. Obtener cliente para notificación
                var cliente = await _clienteRepository.GetByIdAsync(pedido.ClienteId);
                if (cliente != null)
                {
                    // 4. Enviar notificación de cancelación
                    await _servicioNotificacion.EnviarNotificacionCancelacionPedidoAsync(
                        cliente, pedido.NumeroPedido, request.Motivo);
                }

                // 5. Persistir cambios
                await _pedidoRepository.UpdateAsync(pedido);
                await _unitOfWork.CommitTransactionAsync();

                return new CancelarPedidoResult
                {
                    PedidoId = pedido.Id,
                    NumeroPedido = pedido.NumeroPedido,
                    Estado = pedido.Estado.Codigo.ToString(),
                    Motivo = request.Motivo
                };
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }
    }

    public class CancelarPedidoRequest
    {
        public Guid PedidoId { get; set; }
        public string Motivo { get; set; } = string.Empty;
    }

    public class CancelarPedidoResult
    {
        public Guid PedidoId { get; set; }
        public string NumeroPedido { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public string Motivo { get; set; } = string.Empty;
    }
}
