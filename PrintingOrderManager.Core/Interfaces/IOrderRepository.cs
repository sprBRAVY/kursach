using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrintingOrderManager.Core.Entities;

namespace PrintingOrderManager.Core.Interfaces
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<Order?> GetByIdWithDetailsAsync(int id); // Client + OrderItems (+ Service, Worker, Equipment)
        Task<IEnumerable<Order>> GetAllWithDetailsAsync();

        Task<IEnumerable<Order>> GetByClientIdAsync(int clientId);

        Task<IEnumerable<Order>> GetByStatusAsync(string status);

        Task<IEnumerable<Order>> GetBetweenDatesAsync(DateOnly startDate, DateOnly endDate);

        Task<bool> ExistsAsync(int orderId);

        Task<IEnumerable<Order>> GetPagedAsync(int page, int pageSize);

        Task<int> GetCountAsync();
    }
}
