using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using Arquitectura_DDD.Core.Entities;
using Arquitectura_DDD.Core.Interfaces;
using Arquitectura_DDD.Infraestructure.Persistence;

namespace Arquitectura_DDD.Infraestructure.Repositories
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly IMongoCollection<Cliente> _collection;
        private readonly MongoDbContext _context;

        public ClienteRepository(MongoDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _collection = _context.GetCollection<Cliente>("Clientes");
        }

        public IUnitOfWork UnitOfWork => new MongoUnitOfWork(_context);

        public async Task<Cliente> GetByIdAsync(Guid id)
        {
            var filter = Builders<Cliente>.Filter.Eq(c => c.Id, id);
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Cliente>> GetAllAsync()
        {
            return await _collection.Find(_ => true).ToListAsync();
        }

        public async Task<Cliente> GetByEmailAsync(string email)
        {
            var filter = Builders<Cliente>.Filter.Eq(c => c.Email, email);
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task AddAsync(Cliente cliente)
        {
            await _collection.InsertOneAsync(cliente);
        }

        public async Task UpdateAsync(Cliente cliente)
        {
            var filter = Builders<Cliente>.Filter.Eq(c => c.Id, cliente.Id);
            await _collection.ReplaceOneAsync(filter, cliente);
        }

        public async Task DeleteAsync(Guid id)
        {
            var filter = Builders<Cliente>.Filter.Eq(c => c.Id, id);
            await _collection.DeleteOneAsync(filter);
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            var filter = Builders<Cliente>.Filter.Eq(c => c.Id, id);
            var count = await _collection.CountDocumentsAsync(filter);
            return count > 0;
        }
    }
}