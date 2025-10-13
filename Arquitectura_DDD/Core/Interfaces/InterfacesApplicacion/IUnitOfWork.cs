namespace Arquitectura_DDD.Core.Interfaces.InterfacesApplicacion
{
    public interface IUnitOfWork
    {
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}
