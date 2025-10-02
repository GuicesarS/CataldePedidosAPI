using Catalde.Pedidos.Domain.Entities;
using Catalde.Pedidos.Infrastructure.Context;
using Catalde.Pedidos.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Catalde.Pedidos.Infrastructure.Repositories.Implementations;

public class PedidoRepository : IPedidoRepository
{
    private readonly PedidoDbContext _dbContext;
    public PedidoRepository(PedidoDbContext dbContext)
    {
        _dbContext = dbContext;

    }
    public async Task<List<Pedido>> GetAllAsync() => await _dbContext.Pedidos
            .Include(p => p.Ocorrencias)
            .AsNoTracking()
            .ToListAsync();

    public async Task<Pedido?> GetByIdAsync(int id)
    {
       var pedido = await _dbContext.Pedidos
            .Include(p => p.Ocorrencias)
            .FirstOrDefaultAsync(p => p.IdPedido == id);

        return pedido;
    }

    public void Create(Pedido pedido) =>
       _dbContext.Pedidos.Add(pedido);

    public void Update(Pedido pedido) =>
        _dbContext.Pedidos.Update(pedido);

    public void Delete(Pedido pedido) =>
        _dbContext.Pedidos.Remove(pedido);
}
