using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Arquitectura_DDD.Application.DTOs;
using Arquitectura_DDD.Application.UseCases;

namespace Arquitectura_DDD.APIs.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PedidosController : ControllerBase
    {
        private readonly CrearPedidoUseCase _crearPedidoUseCase;
        private readonly ConfirmarPagoUseCase _confirmarPagoUseCase;
        private readonly CancelarPedidoUseCase _cancelarPedidoUseCase;
        private readonly ILogger<PedidosController> _logger;

        public PedidosController(
            CrearPedidoUseCase crearPedidoUseCase,
            ConfirmarPagoUseCase confirmarPagoUseCase,
            CancelarPedidoUseCase cancelarPedidoUseCase,
            ILogger<PedidosController> logger)
        {
            _crearPedidoUseCase = crearPedidoUseCase ?? throw new ArgumentNullException(nameof(crearPedidoUseCase));
            _confirmarPagoUseCase = confirmarPagoUseCase ?? throw new ArgumentNullException(nameof(confirmarPagoUseCase));
            _cancelarPedidoUseCase = cancelarPedidoUseCase ?? throw new ArgumentNullException(nameof(cancelarPedidoUseCase));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost]
        public async Task<IActionResult> CrearPedido([FromBody] CrearPedidoRequestDto request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var useCaseRequest = new CrearPedidoRequest
                {
                    ClienteId = request.ClienteId,
                    Detalles = request.Detalles.Select(d => new DetallePedidoRequest
                    {
                        ProductoId = d.ProductoId,
                        NombreProducto = d.NombreProducto,
                        Cantidad = d.Cantidad,
                        PrecioUnitario = d.PrecioUnitario
                    }).ToList()
                };

                var result = await _crearPedidoUseCase.ExecuteAsync(useCaseRequest);

                return CreatedAtAction(nameof(GetPedido), new { id = result.PedidoId }, result);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning("Error de operación: {Message}", ex.Message);
                return BadRequest(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error interno del servidor al crear pedido");
                return StatusCode(500, new { 
                    Error = "Error interno del servidor",
                    Message = ex.Message,
                    StackTrace = ex.StackTrace
                });
            }
        }

        [HttpPost("{id}/pago")]
        public async Task<IActionResult> ConfirmarPago(Guid id, [FromBody] ConfirmarPagoRequestDto request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (id != request.PedidoId)
                    return BadRequest(new { Error = "El ID del pedido no coincide" });

                var useCaseRequest = new ConfirmarPagoRequest
                {
                    PedidoId = request.PedidoId,
                    TipoPago = Enum.Parse<Arquitectura_DDD.Core.ValueObjects.MetodoPago.TipoPago>(request.TipoPago),
                    Proveedor = request.Proveedor,
                    NumeroReferencia = request.NumeroReferencia
                };

                var result = await _confirmarPagoUseCase.ExecuteAsync(useCaseRequest);

                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { Error = "Error interno del servidor" });
            }
        }

        [HttpPut("{id}/cancelar")]
        public async Task<IActionResult> CancelarPedido(Guid id, [FromBody] CancelarPedidoRequestDto request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (id != request.PedidoId)
                    return BadRequest(new { Error = "El ID del pedido no coincide" });

                var useCaseRequest = new CancelarPedidoRequest
                {
                    PedidoId = request.PedidoId,
                    Motivo = request.Motivo
                };

                var result = await _cancelarPedidoUseCase.ExecuteAsync(useCaseRequest);

                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { Error = "Error interno del servidor" });
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetPedido(Guid id)
        {
            // Implementar consulta de pedido
            return Ok(new { Message = "Consulta de pedido no implementada" });
        }

        [HttpGet("test")]
        public IActionResult TestEndpoint()
        {
            return Ok(new { 
                Message = "Endpoint funcionando", 
                Timestamp = DateTime.UtcNow,
                Environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Unknown"
            });
        }
    }
}
