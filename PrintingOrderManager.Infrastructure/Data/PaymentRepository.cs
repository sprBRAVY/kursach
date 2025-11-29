using Microsoft.EntityFrameworkCore;
using PrintingOrderManager.Core.Entities;
using PrintingOrderManager.Core.Interfaces;
using PrintingOrderManager.Infrastructure.Data;

namespace PrintingOrderManager.Infrastructure.Repositories
{
    public class PaymentRepository : GenericRepository<Payment>, IPaymentRepository
    {
        private readonly ApplicationDbContext _context;

        public PaymentRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Payment?> GetByIdWithOrderAsync(int id)
        {
            return await _context.Payments
                .Include(p => p.Order)
                .ThenInclude(o => o.Client)
                .FirstOrDefaultAsync(p => p.PaymentId == id);
        }

        public async Task<IEnumerable<Payment>> GetAllWithOrderAsync()
        {
            return await _context.Payments
                .Include(p => p.Order)
                .ThenInclude(o => o.Client)
                .ToListAsync();
        }

        public async Task<IEnumerable<Payment>> GetByOrderIdAsync(int orderId)
        {
            return await _context.Payments
                .Where(p => p.OrderId == orderId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Payment>> GetByStatusAsync(string status)
        {
            return await _context.Payments
                .Where(p => p.PaymentStatus == status)
                .ToListAsync();
        }

        public async Task<decimal> GetTotalPaidForOrderAsync(int orderId)
        {
            return await _context.Payments
                .Where(p => p.OrderId == orderId)
                .SumAsync(p => p.Amount);
        }

        public async Task<bool> ExistsAsync(int paymentId)
        {
            return await _context.Payments.AnyAsync(p => p.PaymentId == paymentId);
        }

        public async Task<IEnumerable<Payment>> GetPagedAsync(int page, int pageSize)
        {
            return await _context.Payments
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<int> GetCountAsync()
        {
            return await _context.Payments.CountAsync();
        }
    }
}