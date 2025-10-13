using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Arquitectura_DDD.Core.Entities;

namespace Arquitectura_DDD.Core.Interfaces
{
    public interface IClienteRepository : IRepository<Cliente>
    {
        Task<Cliente> GetByEmailAsync(string email);
        Task<bool> ExistsAsync(Guid id);
    }
}
