using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Arquitectura_DDD.Core.Entities;
using Arquitectura_DDD.Core.ValueObjects;

namespace Arquitectura_DDD.Infraestructure.Configuration
{
    public class ClienteConfiguration : IEntityTypeConfiguration<Cliente>
    {
        public void Configure(EntityTypeBuilder<Cliente> builder)
        {
            builder.ToTable("Clientes");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id)
                .IsRequired();

            builder.Property(c => c.Nombre)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(c => c.Email)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(c => c.Telefono)
                .HasMaxLength(20);

            builder.Property(c => c.LimiteCredito)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(c => c.Activo)
                .IsRequired();

            // Configurar Value Object DireccionEntrega
            builder.OwnsOne(c => c.DireccionEntrega, direccion =>
            {
                direccion.Property(d => d.Calle)
                    .HasColumnName("DireccionCalle")
                    .IsRequired()
                    .HasMaxLength(200);

                direccion.Property(d => d.Ciudad)
                    .HasColumnName("DireccionCiudad")
                    .IsRequired()
                    .HasMaxLength(100);

                direccion.Property(d => d.Departamento)
                    .HasColumnName("DireccionDepartamento")
                    .IsRequired()
                    .HasMaxLength(100);

                direccion.Property(d => d.CodigoPostal)
                    .HasColumnName("DireccionCodigoPostal")
                    .IsRequired()
                    .HasMaxLength(10);

                direccion.Property(d => d.Referencias)
                    .HasColumnName("DireccionReferencias")
                    .HasMaxLength(500);
            });
        }
    }
}
