using Catalde.Pedidos.Infrastructure.Context;
using Catalde.Pedidos.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Catalde.Pedidos.Infrastructure.Repositories.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly PedidoDbContext _context;
        public IPedidoRepository Pedidos { get; }

        public UnitOfWork(PedidoDbContext context, IPedidoRepository pedidoRepository)
        {
            _context = context;
            Pedidos = pedidoRepository;
        }
        public Task<int> CommitAsync()
        {
            return _context.SaveChangesAsync();

        }
    }
}
