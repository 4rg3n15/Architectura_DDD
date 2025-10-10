using System.Threading;
using System.Threading.Tasks;

namespace Arquitectura_DDD.Application.Common
{
    public interface IRequest<TResponse>
    {
    }

    public interface IRequest : IRequest<Unit>
    {
    }

    public interface IRequestHandler<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
    }

    public interface IRequestHandler<TRequest> : IRequestHandler<TRequest, Unit> where TRequest : IRequest
    {
    }

    public struct Unit
    {
        public static readonly Unit Value = new();
    }
}
