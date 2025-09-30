using Catalde.Pedidos.Domain.Entities;

namespace Catalde.Pedidos.Infrastructure.Repositories;

public interface IPedidoRepository
{
    Task<List<Pedido>> GetAllAsync();
    Task<Pedido> GetByIdAsync(int id);
    Task CreateAsync(Pedido pedido);
    Task UpdateAsync(Pedido pedido);
    Task DeleteAsync(Pedido pedido);
    Task SaveChangesAsync();
}
