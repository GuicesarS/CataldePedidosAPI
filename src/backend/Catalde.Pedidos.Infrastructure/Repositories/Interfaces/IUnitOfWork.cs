namespace Catalde.Pedidos.Infrastructure.Repositories.Interfaces;

public interface IUnitOfWork
{
    IPedidoRepository Pedidos { get; }
    Task<int> CommitAsync();
}
