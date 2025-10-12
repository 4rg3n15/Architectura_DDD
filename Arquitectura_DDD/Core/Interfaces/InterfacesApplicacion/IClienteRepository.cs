using System;
using System.Threading.Tasks;
using Arquitectura_DDD.Core.Entities;

namespace Arquitectura_DDD.Core.Interfaces
{
    public interface IClienteRepository
    {
        Task<Cliente> GetByIdAsync(Guid id);
        Task<Cliente> GetByEmailAsync(string email);
        Task<IEnumerable<Cliente>> GetAllAsync();
        Task AddAsync(Cliente cliente);
        Task UpdateAsync(Cliente cliente);
        Task<bool> ExistsAsync(Guid id);
    }
}
