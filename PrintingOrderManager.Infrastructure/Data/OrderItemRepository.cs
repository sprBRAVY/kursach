using Microsoft.EntityFrameworkCore;
using PrintingOrderManager.Core.Entities;
using PrintingOrderManager.Core.Interfaces;
using PrintingOrderManager.Infrastructure.Data;

namespace PrintingOrderManager.Infrastructure.Repositories
{
    public class OrderItemRepository : GenericRepository<OrderItem>, IOrderItemRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderItemRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<OrderItem?> GetByIdWithDetailsAsync(int id)
        {
            return await _context.OrderItems
                .Include(oi => oi.Order)
                .Include(oi => oi.Service)
                .Include(oi => oi.Worker)
                .Include(oi => oi.Equipment)
                .FirstOrDefaultAsync(oi => oi.ItemId == id);
        }

        public async Task<IEnumerable<OrderItem>> GetAllWithDetailsAsync()
        {
            return await _context.OrderItems
                .Include(oi => oi.Service)
                .Include(oi => oi.Worker)
                .Include(oi => oi.Equipment)
                .Include(oi => oi.Order)
                .ToListAsync();
        }

        public async Task<IEnumerable<OrderItem>> GetByOrderIdAsync(int orderId)
        {
            return await _context.OrderItems
                .Where(oi => oi.OrderId == orderId)
                .ToListAsync();
        }

        public async Task<IEnumerable<OrderItem>> GetByServiceIdAsync(int serviceId)
        {
            return await _context.OrderItems
                .Where(oi => oi.ServiceId == serviceId)
                .ToListAsync();
        }

        public async Task<IEnumerable<OrderItem>> GetByWorkerIdAsync(int workerId)
        {
            return await _context.OrderItems
                .Where(oi => oi.WorkerId == workerId)
                .ToListAsync();
        }

        public async Task<IEnumerable<OrderItem>> GetByEquipmentIdAsync(int equipmentId)
        {
            return await _context.OrderItems
                .Where(oi => oi.EquipmentId == equipmentId)
                .ToListAsync();
        }

        public async Task<bool> ExistsAsync(int itemId)
        {
            return await _context.OrderItems.AnyAsync(oi => oi.ItemId == itemId);
        }

        public async Task<IEnumerable<OrderItem>> GetPagedAsync(int page, int pageSize)
        {
            return await _context.OrderItems
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<int> GetCountAsync()
        {
            return await _context.OrderItems.CountAsync();
        }
    }
}