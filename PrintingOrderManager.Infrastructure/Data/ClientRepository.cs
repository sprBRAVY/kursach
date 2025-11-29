using Microsoft.EntityFrameworkCore;
using PrintingOrderManager.Core.Entities;
using PrintingOrderManager.Core.Interfaces;
using PrintingOrderManager.Infrastructure.Data;

namespace PrintingOrderManager.Infrastructure.Repositories
{
    public class ClientRepository : GenericRepository<Client>, IClientRepository
    {
        private readonly ApplicationDbContext _context;

        public ClientRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Client?> GetByNameAsync(string clientName)
        {
            return await _context.Clients
                .FirstOrDefaultAsync(c => c.ClientName == clientName);
        }

        public async Task<Client?> GetWithOrdersAsync(int id)
        {
            return await _context.Clients
                .Include(c => c.Orders)
                .ThenInclude(o => o.OrderItems)
                .ThenInclude(oi => oi.Service)
                .FirstOrDefaultAsync(c => c.ClientId == id);
        }

        public async Task<IEnumerable<Client>> GetAllWithOrdersAsync()
        {
            return await _context.Clients
                .Include(c => c.Orders)
                .ToListAsync();
        }

        public async Task<bool> ExistsByNameAsync(string clientName)
        {
            return await _context.Clients
                .AnyAsync(c => c.ClientName == clientName);
        }

        public async Task<IEnumerable<Client>> GetPagedAsync(int page, int pageSize)
        {
            return await _context.Clients
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<int> GetCountAsync()
        {
            return await _context.Clients.CountAsync();
        }
    }
}