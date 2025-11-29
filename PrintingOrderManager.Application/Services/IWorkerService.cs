// PrintingOrderManager.Application/Services/IWorkerService.cs
using PrintingOrderManager.Core.DTOs;

namespace PrintingOrderManager.Application.Services
{
    public interface IWorkerService
    {
        Task<IEnumerable<WorkerDto>> GetAllWorkersAsync();
        Task<WorkerDto> GetWorkerByIdAsync(int id);
        Task AddWorkerAsync(CreateWorkerDto workerDto);
        Task UpdateWorkerAsync(int id, UpdateWorkerDto workerDto);
        Task DeleteWorkerAsync(int id);
        Task<WorkerDto?> GetWorkerByNameAsync(string name);
        Task<IEnumerable<WorkerDto>> GetWorkersByPositionAsync(string position);
    }
}