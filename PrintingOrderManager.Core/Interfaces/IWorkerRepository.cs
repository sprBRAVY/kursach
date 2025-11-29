using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrintingOrderManager.Core.Entities;

namespace PrintingOrderManager.Core.Interfaces
{
    public interface IWorkerRepository : IGenericRepository<Worker>
    {
        Task<Worker?> GetByNameAsync(string workerFullName);
        Task<Worker?> GetWithAssignedOrdersAsync(int id); // с OrderItems
        Task<IEnumerable<Worker>> GetAllWithAssignedOrdersAsync();

        Task<bool> ExistsByNameAsync(string workerFullName);
        Task<bool> ExistsAsync(int workerId);

        Task<IEnumerable<Worker>> GetByPositionAsync(string position);

        Task<IEnumerable<Worker>> GetPagedAsync(int page, int pageSize);
        Task<int> GetCountAsync();
    }
}
