using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrintingOrderManager.Core.Entities;

namespace PrintingOrderManager.Core.Interfaces
{
    public interface IPaymentRepository : IGenericRepository<Payment>
    {
        Task<Payment?> GetByIdWithOrderAsync(int id);
        Task<IEnumerable<Payment>> GetAllWithOrderAsync();

        Task<IEnumerable<Payment>> GetByOrderIdAsync(int orderId);
        Task<IEnumerable<Payment>> GetByStatusAsync(string status);
        Task<decimal> GetTotalPaidForOrderAsync(int orderId);

        Task<bool> ExistsAsync(int paymentId);

        Task<IEnumerable<Payment>> GetPagedAsync(int page, int pageSize);
        Task<int> GetCountAsync();
    }
}
