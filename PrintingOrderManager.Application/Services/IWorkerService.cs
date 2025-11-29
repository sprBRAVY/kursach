// PrintingOrderManager.Application/Services/IWorkerService.cs
using PrintingOrderManager.Core.DTOs;
using System.Linq;

namespace PrintingOrderManager.Application.Services
{
    public interface IWorkerService
    {
        Task<IEnumerable<WorkerDto>> GetAllWorkersAsync();
        IQueryable<WorkerDto> GetWorkersQueryable(); // ← НОВЫЙ МЕТОД
        Task<WorkerDto> GetWorkerByIdAsync(int id);
        Task AddWorkerAsync(CreateWorkerDto workerDto);
        Task UpdateWorkerAsync(int id, UpdateWorkerDto workerDto);
        Task DeleteWorkerAsync(int id);
        Task<WorkerDto?> GetWorkerByNameAsync(string name);
        Task<IEnumerable<WorkerDto>> GetWorkersByPositionAsync(string position);
    }
}