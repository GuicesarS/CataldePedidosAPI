using Catalde.Pedidos.Domain.Entities;
using Catalde.Pedidos.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Catalde.Pedidos.Infrastructure.Context;

public class PedidoDbContext: DbContext
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

        base.OnModelCreating(modelBuilder);
    }
}
