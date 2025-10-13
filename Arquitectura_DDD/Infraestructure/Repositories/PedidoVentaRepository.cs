using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using Arquitectura_DDD.Core.Aggregates;
using Arquitectura_DDD.Core.Interfaces;
using Arquitectura_DDD.Core.ValueObjects;
using Arquitectura_DDD.Infraestructure.Persistence;

namespace Arquitectura_DDD.Infraestructure.Repositories
{
    public class PedidoVentaRepository : IPedidoVentaRepository
    {
        private readonly IMongoCollection<PedidoVenta> _collection;
        private readonly MongoDbContext _context;

        public PedidoVentaRepository(MongoDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _collection = _context.GetCollection<PedidoVenta>("PedidosVenta");
        }

        public IUnitOfWork UnitOfWork => new MongoUnitOfWork(_context);

        public async Task<PedidoVenta> GetByIdAsync(Guid id)
        {
            var filter = Builders<PedidoVenta>.Filter.Eq(p => p.Id, id);
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<PedidoVenta>> GetAllAsync()
        {
            return await _collection.Find(_ => true).ToListAsync();
        }

        public async Task<PedidoVenta> GetByNumeroAsync(string numeroPedido)
        {
            var filter = Builders<PedidoVenta>.Filter.Eq(p => p.NumeroPedido, numeroPedido);
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<PedidoVenta>> GetByClienteIdAsync(Guid clienteId)
        {
            var filter = Builders<PedidoVenta>.Filter.Eq(p => p.ClienteId, clienteId);
            return await _collection.Find(filter).ToListAsync();
        }

        public async Task AddAsync(PedidoVenta pedido)
        {
            await _collection.InsertOneAsync(pedido);
        }

        public async Task UpdateAsync(PedidoVenta pedido)
        {
            var filter = Builders<PedidoVenta>.Filter.Eq(p => p.Id, pedido.Id);
            await _collection.ReplaceOneAsync(filter, pedido);
        }

        public async Task DeleteAsync(Guid id)
        {
            var filter = Builders<PedidoVenta>.Filter.Eq(p => p.Id, id);
            await _collection.DeleteOneAsync(filter);
        }

        public async Task<IEnumerable<PedidoVenta>> GetPedidosPendientesAsync()
        {
            var filter = Builders<PedidoVenta>.Filter.Eq(p => p.Estado.Codigo, EstadoPedido.CodigoEstado.Pendiente);
            return await _collection.Find(filter).ToListAsync();
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            var filter = Builders<PedidoVenta>.Filter.Eq(p => p.Id, id);
            var count = await _collection.CountDocumentsAsync(filter);
            return count > 0;
        }
    }
}