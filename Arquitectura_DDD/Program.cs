using Microsoft.EntityFrameworkCore;
using Arquitectura_DDD.Infraestructure.Persistence;
using Arquitectura_DDD.Core.Interfaces;
using Arquitectura_DDD.Infraestructure.Repositories;
using Arquitectura_DDD.Core.Services;
using Arquitectura_DDD.Application.UseCases;
using Arquitectura_DDD.Infraestructure.Cache;

namespace Arquitectura_DDD
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Configurar Entity Framework
            builder.Services.AddDbContext<VentasDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            
            // Registrar IUnitOfWork
            builder.Services.AddScoped<IUnitOfWork>(provider => provider.GetRequiredService<VentasDbContext>());

            // Configurar caché en memoria
            builder.Services.AddMemoryCache();
            builder.Services.AddScoped<ICacheService, CacheService>();

            // Registrar repositorios
            builder.Services.AddScoped<IPedidoVentaRepository, PedidoVentaRepository>();
            builder.Services.AddScoped<IClienteRepository, ClienteRepository>();

            // Registrar servicios de dominio
            builder.Services.AddScoped<IServicioValidacionCredito, ServicioValidacionCredito>();
            builder.Services.AddScoped<IServicioNotificacionClientes, ServicioNotificacionClientes>();
            builder.Services.AddScoped<ServicioGestionPedidos>();
            builder.Services.AddScoped<ServicioProcesamientoVentas>();

            // Registrar use cases
            builder.Services.AddScoped<CrearPedidoUseCase>();
            builder.Services.AddScoped<CancelarPedidoUseCase>();
            builder.Services.AddScoped<ConfirmarPagoUseCase>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}