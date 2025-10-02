using Catalde.Pedidos.Domain.Entities;

namespace Catalde.Pedidos.Infrastructure.Repositories.Interfaces;

public interface IPedidoRepository
{
    Task<List<Pedido>> GetAllAsync();
    Task<Pedido?> GetByIdAsync(int id);
    void Create(Pedido pedido);
    void Update(Pedido pedido);
    void Delete(Pedido pedido);
}
