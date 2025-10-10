using System;
using System.Threading.Tasks;
using Arquitectura_DDD.Core.Entities;
using Arquitectura_DDD.Core.ValueObjects;

namespace Arquitectura_DDD.Core.Interfaces
{
    public interface IServicioNotificacionClientes
    {
        Task EnviarNotificacionConfirmacionPedidoAsync(Cliente cliente, string numeroPedido);
        Task EnviarNotificacionCancelacionPedidoAsync(Cliente cliente, string numeroPedido, string motivo);
        Task EnviarNotificacionDespachoAsync(Cliente cliente, string numeroPedido);
        Task EnviarNotificacionEntregaAsync(Cliente cliente, string numeroPedido);
        Task EnviarNotificacionAsync(CanalNotificacion canal, string mensaje);
    }
}
