using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrintingOrderManager.Core.Entities;

namespace PrintingOrderManager.Core.Interfaces
{
    public interface IServiceRepository : IGenericRepository<Service>
    {
        Task<Service?> GetByNameAsync(string serviceName);
        Task<Service?> GetWithUsageAsync(int id); // с OrderItems
        Task<IEnumerable<Service>> GetAllWithUsageAsync();

        Task<bool> ExistsByNameAsync(string serviceName);
        Task<bool> ExistsAsync(int serviceId);

        Task<IEnumerable<Service>> GetByPriceRangeAsync(decimal minPrice, decimal maxPrice);

        Task<IEnumerable<Service>> GetPagedAsync(int page, int pageSize);
        Task<int> GetCountAsync();
    }
}
