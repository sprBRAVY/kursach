// PrintingOrderManager.Core.Interfaces/IClientRepository.cs
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PrintingOrderManager.Core.Entities;

namespace PrintingOrderManager.Core.Interfaces
{
    public interface IClientRepository : IGenericRepository<Client>
    {
        Task<Client?> GetByNameAsync(string clientName);
        Task<Client?> GetWithOrdersAsync(int id);
        Task<IEnumerable<Client>> GetAllWithOrdersAsync();
        Task<bool> ExistsByNameAsync(string clientName);
        Task<IEnumerable<Client>> GetPagedAsync(int page, int pageSize);
        Task<int> GetCountAsync();
    }
}