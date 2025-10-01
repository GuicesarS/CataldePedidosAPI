using Catalde.Pedidos.Domain.Entities;
using Catalde.Pedidos.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Catalde.Pedidos.Infrastructure.Repositories;

public class PedidoRepository : IPedidoRepository
{
    private readonly PedidoDbContext _dbContext;
    public PedidoRepository(PedidoDbContext dbContext)
    {
        _dbContext = dbContext;

    }
    public async Task<List<Pedido>> GetAllAsync() => await _dbContext.Pedidos
            .Include(p => p.Ocorrencias)
            .ToListAsync();

    public async Task<Pedido> GetByIdAsync(int id)
    {
       var pedidos = await _dbContext.Pedidos
            .Include(p => p.Ocorrencias)
            .FirstOrDefaultAsync(p => p.IdPedido == id);

        return pedidos;
    }

    public async Task CreateAsync(Pedido pedido)
    {
        await _dbContext.Pedidos.AddAsync(pedido);
        await _dbContext.SaveChangesAsync();
    }
    
    public async Task UpdateAsync(Pedido pedido)
    {
       _dbContext.Pedidos.Update(pedido);
        await _dbContext.SaveChangesAsync();
    }
     public Task DeleteAsync(Pedido pedido)
    {
        _dbContext.Pedidos.Remove(pedido);
        return Task.CompletedTask;
    }
    public async Task SaveChangesAsync() => await _dbContext.SaveChangesAsync();
}
