using Microsoft.EntityFrameworkCore;
using PrintingOrderManager.Core.Entities;
using PrintingOrderManager.Core.Interfaces;
using PrintingOrderManager.Infrastructure.Data;

namespace PrintingOrderManager.Infrastructure.Repositories
{
    public class WorkerRepository : GenericRepository<Worker>, IWorkerRepository
    {
        private readonly ApplicationDbContext _context;

        public WorkerRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Worker?> GetByNameAsync(string workerFullName)
        {
            return await _context.Workers
                .FirstOrDefaultAsync(w => w.WorkerFullName == workerFullName);
        }

        public async Task<Worker?> GetWithAssignedOrdersAsync(int id)
        {
            return await _context.Workers
                .Include(w => w.OrderItems)
                .ThenInclude(oi => oi.Order)
                .FirstOrDefaultAsync(w => w.WorkerId == id);
        }

        public async Task<IEnumerable<Worker>> GetAllWithAssignedOrdersAsync()
        {
            return await _context.Workers
                .Include(w => w.OrderItems)
                .ToListAsync();
        }

        public async Task<bool> ExistsByNameAsync(string workerFullName)
        {
            return await _context.Workers
                .AnyAsync(w => w.WorkerFullName == workerFullName);
        }

        public async Task<bool> ExistsAsync(int workerId)
        {
            return await _context.Workers.AnyAsync(w => w.WorkerId == workerId);
        }

        public async Task<IEnumerable<Worker>> GetByPositionAsync(string position)
        {
            return await _context.Workers
                .Where(w => w.Position == position)
                .ToListAsync();
        }

        public async Task<IEnumerable<Worker>> GetPagedAsync(int page, int pageSize)
        {
            return await _context.Workers
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<int> GetCountAsync()
        {
            return await _context.Workers.CountAsync();
        }
    }
}