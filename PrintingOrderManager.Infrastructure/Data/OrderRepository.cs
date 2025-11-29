using Microsoft.EntityFrameworkCore;
using PrintingOrderManager.Core.Entities;
using PrintingOrderManager.Core.Interfaces;
using PrintingOrderManager.Infrastructure.Data;

namespace PrintingOrderManager.Infrastructure.Repositories
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Order?> GetByIdWithDetailsAsync(int id)
        {
            return await _context.Orders
                .Include(o => o.Client)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Service)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Worker)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Equipment)
                .Include(o => o.Payments)
                .FirstOrDefaultAsync(o => o.OrderId == id);
        }

        public async Task<IEnumerable<Order>> GetAllWithDetailsAsync()
        {
            return await _context.Orders
                .Include(o => o.Client)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Service)
                .Include(o => o.Payments)
                .ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetByClientIdAsync(int clientId)
        {
            return await _context.Orders
                .Where(o => o.ClientId == clientId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetByStatusAsync(string status)
        {
            return await _context.Orders
                .Where(o => o.Status == status)
                .ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetBetweenDatesAsync(DateOnly start, DateOnly end)
        {
            return await _context.Orders
                .Where(o => o.PlacementDate >= start && o.PlacementDate <= end)
                .ToListAsync();
        }

        public async Task<bool> ExistsAsync(int orderId)
        {
            return await _context.Orders.AnyAsync(o => o.OrderId == orderId);
        }

        public async Task<IEnumerable<Order>> GetPagedAsync(int page, int pageSize)
        {
            return await _context.Orders
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<int> GetCountAsync()
        {
            return await _context.Orders.CountAsync();
        }
    }
}