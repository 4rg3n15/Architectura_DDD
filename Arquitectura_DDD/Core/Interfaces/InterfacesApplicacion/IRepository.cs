using System;
using System.Threading.Tasks;
using Arquitectura_DDD.Core.Common;

namespace Arquitectura_DDD.Core.Interfaces
{
    public interface IRepository<T> where T : IAggregateRoot
    {
        Task<T> GetByIdAsync(Guid id);
        Task<IEnumerable<T>> GetAllAsync();
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(Guid id);
        IUnitOfWork UnitOfWork { get; }
    }

    public interface IUnitOfWork : IDisposable
    {
        Task<int> SaveChangesAsync();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}
