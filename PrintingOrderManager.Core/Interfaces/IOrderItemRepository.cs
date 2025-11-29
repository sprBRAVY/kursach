using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrintingOrderManager.Core.Entities;

namespace PrintingOrderManager.Core.Interfaces
{
    public interface IOrderItemRepository : IGenericRepository<OrderItem>
    {
        Task<OrderItem?> GetByIdWithDetailsAsync(int id); 
        Task<IEnumerable<OrderItem>> GetAllWithDetailsAsync();

        Task<IEnumerable<OrderItem>> GetByOrderIdAsync(int orderId);
        Task<IEnumerable<OrderItem>> GetByServiceIdAsync(int serviceId);
        Task<IEnumerable<OrderItem>> GetByWorkerIdAsync(int workerId);
        Task<IEnumerable<OrderItem>> GetByEquipmentIdAsync(int equipmentId);

        Task<bool> ExistsAsync(int itemId);

        Task<IEnumerable<OrderItem>> GetPagedAsync(int page, int pageSize);
        Task<int> GetCountAsync();
    }
}
