using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Arquitectura_DDD.Core.Aggregates;
using Arquitectura_DDD.Core.ValueObjects;

namespace Arquitectura_DDD.Infraestructure.Configuration
{
    public class PedidoVentaConfiguration : IEntityTypeConfiguration<PedidoVenta>
    {
        public void Configure(EntityTypeBuilder<PedidoVenta> builder)
        {
            builder.ToTable("PedidosVenta");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.NumeroPedido)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(p => p.ClienteId)
                .IsRequired();

            builder.Property(p => p.FechaCreacion)
                .IsRequired();

            // Configurar Value Object EstadoPedido
            builder.OwnsOne(p => p.Estado, estado =>
            {
                estado.Property(e => e.Codigo)
                    .HasColumnName("EstadoCodigo")
                    .HasConversion<int>();

                estado.Property(e => e.FechaActualizacion)
                    .HasColumnName("EstadoFechaActualizacion");
            });

            // Value Objects como owned types
            builder.OwnsOne(p => p.MetodoPago, mp =>
            {
                mp.Property(m => m.Tipo)
                    .HasColumnName("MetodoPagoTipo")
                    .HasConversion<string>();
                mp.Property(m => m.Proveedor)
                    .HasColumnName("MetodoPagoProveedor")
                    .HasMaxLength(100);
                mp.Property(m => m.NumeroReferencia)
                    .HasColumnName("MetodoPagoNumeroReferencia")
                    .HasMaxLength(100);
            });

            builder.OwnsOne(p => p.MontoTotal, mt =>
            {
                mt.Property(m => m.Subtotal)
                    .HasColumnName("MontoSubtotal")
                    .HasColumnType("decimal(18,2)");
                mt.Property(m => m.Impuestos)
                    .HasColumnName("MontoImpuestos")
                    .HasColumnType("decimal(18,2)");
                mt.Property(m => m.Descuentos)
                    .HasColumnName("MontoDescuentos")
                    .HasColumnType("decimal(18,2)");
                mt.Property(m => m.Total)
                    .HasColumnName("MontoTotal")
                    .HasColumnType("decimal(18,2)");
            });

            // Configurar relación con detalles como owned entities
            builder.OwnsMany(p => p.Detalles, detalle =>
            {
                detalle.WithOwner().HasForeignKey("PedidoVentaId");
                detalle.Property<int>("Id");
                detalle.HasKey("Id");
                
                detalle.Property(d => d.ProductoId)
                    .HasColumnName("ProductoId");
                detalle.Property(d => d.NombreProducto)
                    .HasColumnName("NombreProducto")
                    .HasMaxLength(200);
                detalle.Property(d => d.Cantidad)
                    .HasColumnName("Cantidad");
                detalle.Property(d => d.PrecioUnitario)
                    .HasColumnName("PrecioUnitario")
                    .HasColumnType("decimal(18,2)");
            });
        }
    }
}
