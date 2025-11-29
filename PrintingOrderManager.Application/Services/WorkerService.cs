// PrintingOrderManager.Application/Services/WorkerService.cs
using AutoMapper;
using PrintingOrderManager.Core.DTOs;
using PrintingOrderManager.Core.Entities;
using PrintingOrderManager.Core.Interfaces;

namespace PrintingOrderManager.Application.Services
{
    public class WorkerService : IWorkerService
    {
        private readonly IWorkerRepository _workerRepository;
        private readonly IMapper _mapper;

        public WorkerService(IWorkerRepository workerRepository, IMapper mapper)
        {
            _workerRepository = workerRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<WorkerDto>> GetAllWorkersAsync()
        {
            var workers = await _workerRepository.GetAllWithAssignedOrdersAsync();
            return _mapper.Map<IEnumerable<WorkerDto>>(workers);
        }

        public async Task<WorkerDto> GetWorkerByIdAsync(int id)
        {
            var worker = await _workerRepository.GetByIdAsync(id);
            if (worker == null) return null;
            return _mapper.Map<WorkerDto>(worker);
        }

        public async Task AddWorkerAsync(CreateWorkerDto workerDto)
        {
            var worker = _mapper.Map<Worker>(workerDto);
            await _workerRepository.AddAsync(worker);
        }

        public async Task UpdateWorkerAsync(int id, UpdateWorkerDto workerDto)
        {
            var existingWorker = await _workerRepository.GetByIdAsync(id);
            if (existingWorker == null)
            {
                throw new KeyNotFoundException($"Worker with ID {id} not found.");
            }
            _mapper.Map(workerDto, existingWorker);
            await _workerRepository.UpdateAsync(existingWorker);
        }

        public async Task DeleteWorkerAsync(int id)
        {
            await _workerRepository.DeleteAsync(id);
        }

        public async Task<WorkerDto?> GetWorkerByNameAsync(string name)
        {
            var worker = await _workerRepository.GetByNameAsync(name);
            if (worker == null) return null;
            return _mapper.Map<WorkerDto>(worker);
        }

        public async Task<IEnumerable<WorkerDto>> GetWorkersByPositionAsync(string position)
        {
            var workers = await _workerRepository.GetByPositionAsync(position);
            return _mapper.Map<IEnumerable<WorkerDto>>(workers);
        }
    }
}