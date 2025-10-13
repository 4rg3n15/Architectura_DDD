using System;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using Arquitectura_DDD.Core.Interfaces;

namespace Arquitectura_DDD.Infraestructure.Persistence
{
    public class MongoUnitOfWork : IUnitOfWork
    {
        private readonly MongoDbContext _context;
        private IClientSessionHandle? _session;

        public MongoUnitOfWork(MongoDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task BeginTransactionAsync()
        {
            if (_session == null)
            {
                var client = _context.GetCollection<object>("temp").Database.Client;
                _session = await client.StartSessionAsync();
                _session.StartTransaction();
            }
        }

        public async Task CommitTransactionAsync()
        {
            if (_session != null)
            {
                await _session.CommitTransactionAsync();
                _session.Dispose();
                _session = null;
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (_session != null)
            {
                await _session.AbortTransactionAsync();
                _session.Dispose();
                _session = null;
            }
        }

        public async Task<int> SaveChangesAsync()
        {
            // En MongoDB no necesitamos SaveChanges como en Entity Framework
            // Las operaciones se ejecutan inmediatamente
            await Task.CompletedTask;
            return 1; // Simulamos que se guardó 1 entidad
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            return 1;
        }

        public void Dispose()
        {
            _session?.Dispose();
        }
    }
}
