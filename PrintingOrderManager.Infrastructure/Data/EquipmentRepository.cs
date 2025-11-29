// PrintingOrderManager.Infrastructure.Repositories/EquipmentRepository.cs
using Microsoft.EntityFrameworkCore;
using PrintingOrderManager.Core.Entities;
using PrintingOrderManager.Core.Interfaces;
using PrintingOrderManager.Infrastructure.Data;

namespace PrintingOrderManager.Infrastructure.Repositories
{
    public class EquipmentRepository : GenericRepository<Equipment>, IEquipmentRepository
    {
        private readonly ApplicationDbContext _context;

        public EquipmentRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Equipment?> GetByNameAsync(string equipmentName)
        {
            return await _context.Equipment
                .FirstOrDefaultAsync(e => e.EquipmentName == equipmentName);
        }

        public async Task<Equipment?> GetWithUsageAsync(int id)
        {
            return await _context.Equipment
                .Include(e => e.OrderItems)
                .ThenInclude(oi => oi.Order)
                .FirstOrDefaultAsync(e => e.EquipmentId == id);
        }

        public async Task<IEnumerable<Equipment>> GetAllWithUsageAsync()
        {
            return await _context.Equipment
                .Include(e => e.OrderItems)
                .ToListAsync();
        }

        public async Task<bool> ExistsByNameAsync(string equipmentName)
        {
            return await _context.Equipment
                .AnyAsync(e => e.EquipmentName == equipmentName);
        }

        public async Task<IEnumerable<Equipment>> GetPagedAsync(int page, int pageSize)
        {
            return await _context.Equipment
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<int> GetCountAsync()
        {
            return await _context.Equipment.CountAsync();
        }
    }
}