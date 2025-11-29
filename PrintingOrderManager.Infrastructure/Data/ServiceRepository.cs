using Microsoft.EntityFrameworkCore;
using PrintingOrderManager.Core.Entities;
using PrintingOrderManager.Core.Interfaces;
using PrintingOrderManager.Infrastructure.Data;

namespace PrintingOrderManager.Infrastructure.Repositories
{
    public class ServiceRepository : GenericRepository<Service>, IServiceRepository
    {
        private readonly ApplicationDbContext _context;

        public ServiceRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Service?> GetByNameAsync(string serviceName)
        {
            return await _context.Services
                .FirstOrDefaultAsync(s => s.ServiceName == serviceName);
        }

        public async Task<Service?> GetWithUsageAsync(int id)
        {
            return await _context.Services
                .Include(s => s.OrderItems)
                .ThenInclude(oi => oi.Order)
                .FirstOrDefaultAsync(s => s.ServiceId == id);
        }

        public async Task<IEnumerable<Service>> GetAllWithUsageAsync()
        {
            return await _context.Services
                .Include(s => s.OrderItems)
                .ToListAsync();
        }

        public async Task<bool> ExistsByNameAsync(string serviceName)
        {
            return await _context.Services
                .AnyAsync(s => s.ServiceName == serviceName);
        }

        public async Task<bool> ExistsAsync(int serviceId)
        {
            return await _context.Services.AnyAsync(s => s.ServiceId == serviceId);
        }

        public async Task<IEnumerable<Service>> GetByPriceRangeAsync(decimal minPrice, decimal maxPrice)
        {
            return await _context.Services
                .Where(s => s.UnitPrice >= minPrice && s.UnitPrice <= maxPrice)
                .ToListAsync();
        }

        public async Task<IEnumerable<Service>> GetPagedAsync(int page, int pageSize)
        {
            return await _context.Services
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<int> GetCountAsync()
        {
            return await _context.Services.CountAsync();
        }
    }
}