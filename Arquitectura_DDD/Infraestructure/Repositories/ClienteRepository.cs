using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Arquitectura_DDD.Core.Entities;
using Arquitectura_DDD.Core.Interfaces;
using Arquitectura_DDD.Infraestructure.Persistence;

namespace Arquitectura_DDD.Infraestructure.Repositories
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly VentasDbContext _context;

        public ClienteRepository(VentasDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IUnitOfWork UnitOfWork => _context;

        public async Task<Cliente> GetByIdAsync(Guid id)
        {
            return await _context.Clientes
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Cliente>> GetAllAsync()
        {
            return await _context.Clientes.ToListAsync();
        }

        public async Task<Cliente> GetByEmailAsync(string email)
        {
            return await _context.Clientes
                .FirstOrDefaultAsync(c => c.Email == email);
        }

        public async Task AddAsync(Cliente cliente)
        {
            await _context.Clientes.AddAsync(cliente);
        }

        public async Task UpdateAsync(Cliente cliente)
        {
            _context.Clientes.Update(cliente);
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(Guid id)
        {
            var cliente = await GetByIdAsync(id);
            if (cliente != null)
            {
                _context.Clientes.Remove(cliente);
            }
        }
    }
}
