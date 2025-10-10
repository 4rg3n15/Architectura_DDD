using System;

namespace Arquitectura_DDD.Core.Exceptions
{
    public class DetallePedidoException : Exception
    {
        public DetallePedidoException(string message) : base(message)
        {
        }

        public DetallePedidoException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
