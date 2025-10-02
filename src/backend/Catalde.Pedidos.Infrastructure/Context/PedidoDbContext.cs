using Catalde.Pedidos.Domain.Entities;
using Catalde.Pedidos.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Catalde.Pedidos.Infrastructure.Context;

public class PedidoDbContext : DbContext
{
    public PedidoDbContext(DbContextOptions<PedidoDbContext> options) : base(options) { }
    public DbSet<Pedido> Pedidos { get; set; }
    public DbSet<Ocorrencia> Ocorrencias { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Pedido>(builder =>
        {
            builder.HasKey(p => p.IdPedido);


            builder.OwnsOne(p => p.NumeroPedido, np =>
            {
                np.Property(n => n.Value).HasColumnName("NumeroPedido").IsRequired();

                np.WithOwner().HasForeignKey("PedidoId");

                np.HasData(
                new { PedidoId = 1, Value = 1001 },
                new { PedidoId = 2, Value = 1002 });

            });

            builder.Property(p => p.HorarioPedido)
                   .IsRequired();

            builder.Property(p => p.IndEntregue)
                   .IsRequired();

            builder.HasMany(p => p.Ocorrencias)
                   .WithOne()
                   .HasForeignKey("PedidoId")
                   .OnDelete(DeleteBehavior.Cascade);

            builder.ToTable("Pedidos");

        });

        modelBuilder.Entity<Pedido>().HasData(
         new
         {
             IdPedido = 1,
             HorarioPedido = new DateTime(2025, 10, 01, 09, 00, 00, DateTimeKind.Utc),
             IndEntregue = true
         },
         new
         {
             IdPedido = 2,
             HorarioPedido = new DateTime(2025, 10, 01, 10, 30, 00, DateTimeKind.Utc),
             IndEntregue = false
         }

     );

        modelBuilder.Entity<Ocorrencia>(builder =>
        {
            builder.HasKey(o => o.IdOcorrencia);

            builder.Property(o => o.TipoOcorrencia)
                   .IsRequired();

            builder.Property(o => o.HoraOcorrencia)
                   .IsRequired();

            builder.Property(o => o.IndFinalizadora)
                   .IsRequired();

            builder.ToTable("Ocorrencias");

        });

        modelBuilder.Entity<Ocorrencia>().HasData(
        new
        {
            IdOcorrencia = 1,
            TipoOcorrencia = ETipoOcorrencia.EmRotaDeEntrega,
            HoraOcorrencia = new DateTime(2025, 10, 01, 09, 15, 00, DateTimeKind.Utc),
            IndFinalizadora = false,
            PedidoId = 1
        },
        new
        {
            IdOcorrencia = 2,
            TipoOcorrencia = ETipoOcorrencia.EntregueComSucesso,
            HoraOcorrencia = new DateTime(2025, 10, 01, 09, 20, 00, DateTimeKind.Utc),
            IndFinalizadora = true,
            PedidoId = 1
        },
        new
        {
            IdOcorrencia = 3,
            TipoOcorrencia = ETipoOcorrencia.EmRotaDeEntrega,
            HoraOcorrencia = new DateTime(2025, 10, 01, 10, 35, 00, DateTimeKind.Utc),
            IndFinalizadora = false,
            PedidoId = 2
        },
        new
        {
            IdOcorrencia = 4,
            TipoOcorrencia = ETipoOcorrencia.ClienteAusente,
            HoraOcorrencia = new DateTime(2025, 10, 01, 10, 44, 00, DateTimeKind.Utc),
            IndFinalizadora = true,
            PedidoId = 2
        }
     );

        base.OnModelCreating(modelBuilder);
    }
}
