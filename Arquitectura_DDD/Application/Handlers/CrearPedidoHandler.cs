using Arquitectura_DDD.Application.Commands;
using Arquitectura_DDD.Core.Aggregates;
using Arquitectura_DDD.Core.Interfaces.InterfacesApplicacion;
using Arquitectura_DDD.Core.Interfaces.InterfacesDominio;
using Arquitectura_DDD.Core.ValueObjects;

namespace Arquitectura_DDD.Application.Handlers
{
    public sealed class CrearPedidoHandler : IRequestHandler<CrearPedidoCommand, Guid>
    {
        private readonly IPedidoVentaRepository _pedidoRepository;
        private readonly IClienteRepository _clienteRepository;
        private readonly IServicioValidacionCredito _servicioValidacionCredito;
        private readonly IUnitOfWork _unitOfWork;

        public CrearPedidoHandler(
            IPedidoVentaRepository pedidoRepository,
            IClienteRepository clienteRepository,
            IServicioValidacionCredito servicioValidacionCredito,
            IUnitOfWork unitOfWork)
        {
            _pedidoRepository = pedidoRepository;
            _clienteRepository = clienteRepository;
            _servicioValidacionCredito = servicioValidacionCredito;
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(CrearPedidoCommand request, CancellationToken cancellationToken)
        {
            // 1. Validar cliente existe
            var cliente = await _clienteRepository.GetByIdAsync(request.ClienteId);
            if (cliente == null)
                throw new ApplicationException("Cliente no encontrado");

            // 2. Validar crédito (orquestación entre servicios de dominio)
            var montoTotal = request.Detalles.Sum(d => d.Cantidad * d.PrecioUnitario);
            var creditoValido = await _servicioValidacionCredito.ValidarCreditoClienteAsync(request.ClienteId, montoTotal);
            if (!creditoValido)
                throw new ApplicationException("Crédito insuficiente para el pedido");

            // 3. Crear pedido
            var numeroPedido = $"PED-{DateTime.UtcNow:yyyyMMdd-HHmmss}";
            var pedido = new PedidoVenta(request.ClienteId, numeroPedido, cliente.DireccionEntrega);

            // 4. Agregar detalles
            foreach (var detalleDto in request.Detalles)
            {
                pedido.AgregarDetalle(
                    detalleDto.ProductoId,
                    detalleDto.NombreProducto,
                    detalleDto.Cantidad,
                    detalleDto.PrecioUnitario
                );
            }

            // 5. Procesar pago
            var metodoPago = new MetodoPago(
                Enum.Parse<MetodoPago.TipoPago>(request.MetodoPago.Tipo),
                request.MetodoPago.Proveedor,
                request.MetodoPago.NumeroReferencia
            );

            pedido.ConfirmarPago(metodoPago);

            // 6. Persistir (transacción completa)
            await _pedidoRepository.AddAsync(pedido);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return pedido.Id;
        }
    }
}
