using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrintingOrderManager.Core.Entities;

namespace PrintingOrderManager.Core.Interfaces
{
    public interface IEquipmentRepository : IGenericRepository<Equipment>
    {
        Task<Equipment?> GetByNameAsync(string equipmentName);

        Task<Equipment?> GetWithUsageAsync(int id); // с OrderItems

        Task<IEnumerable<Equipment>> GetAllWithUsageAsync();

        Task<bool> ExistsByNameAsync(string equipmentName);

        Task<IEnumerable<Equipment>> GetPagedAsync(int page, int pageSize);

        Task<int> GetCountAsync();
    }
}
