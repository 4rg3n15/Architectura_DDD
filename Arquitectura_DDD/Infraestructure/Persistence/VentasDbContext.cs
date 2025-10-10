using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using Arquitectura_DDD.Core.Aggregates;
using Arquitectura_DDD.Core.Entities;
using Arquitectura_DDD.Core.Common;
using Arquitectura_DDD.Core.Interfaces;

namespace Arquitectura_DDD.Infraestructure.Persistence
{
    public class VentasDbContext : DbContext, IUnitOfWork
    {
        public DbSet<PedidoVenta> Pedidos { get; set; }
        public DbSet<Cliente> Clientes { get; set; }

        public VentasDbContext(DbContextOptions<VentasDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new PedidoVentaConfiguration());
            modelBuilder.ApplyConfiguration(new ClienteConfiguration());

            base.OnModelCreating(modelBuilder);
        }

        public async Task<int> SaveChangesAsync()
        {
            // Dispatch Domain Events
            await DispatchDomainEventsAsync();

            return await base.SaveChangesAsync();
        }

        public async Task BeginTransactionAsync()
        {
            await Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            await Database.CommitTransactionAsync();
        }

        public async Task RollbackTransactionAsync()
        {
            await Database.RollbackTransactionAsync();
        }

        private async Task DispatchDomainEventsAsync()
        {
            var domainEntities = ChangeTracker
                .Entries<Entity>()
                .Where(x => x.Entity.DomainEvents?.Any() == true)
                .ToList();

            var domainEvents = domainEntities
                .SelectMany(x => x.Entity.DomainEvents)
                .ToList();

            domainEntities.ForEach(entity => entity.Entity.ClearDomainEvents());

            // Publicar eventos (implementar con MediatR o similar)
            foreach (var domainEvent in domainEvents)
            {
                // await _domainEventPublisher.Publish(domainEvent);
                // Por ahora solo logueamos el evento
                Console.WriteLine($"Domain Event: {domainEvent.GetType().Name}");
            }
        }
    }
}
