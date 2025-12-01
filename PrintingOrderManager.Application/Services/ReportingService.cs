using AutoMapper;
using PrintingOrderManager.Core.DTOs;
using PrintingOrderManager.Core.Entities;
using PrintingOrderManager.Core.Interfaces;

public class ReportingService : IReportingService
{
    private readonly IOrderItemRepository _orderItemRepository;
    private readonly IMapper _mapper;

    public ReportingService(IOrderItemRepository orderItemRepository, IMapper mapper)
    {
        _orderItemRepository = orderItemRepository;
        _mapper = mapper;
    }

    public async Task<List<EquipmentUtilizationDto>> GetEquipmentUtilizationAsync()
    {
        var items = await _orderItemRepository.GetAllWithDetailsAsync();
        return items
            .Where(x => x.EquipmentId.HasValue)
            .GroupBy(x => new { x.EquipmentId, x.Equipment!.EquipmentName })
            .Select(g => new EquipmentUtilizationDto
            {
                EquipmentId = g.Key.EquipmentId!.Value,
                EquipmentName = g.Key.EquipmentName,
                TotalTasks = g.Count(),
                TotalItems = g.Sum(x => x.Quantity)
            })
            .ToList();
    }

    public async Task<List<WorkerUtilizationDto>> GetWorkerUtilizationAsync()
    {
        var items = await _orderItemRepository.GetAllWithDetailsAsync();
        return items
            .Where(x => x.WorkerId.HasValue)
            .GroupBy(x => new { x.WorkerId, x.Worker!.WorkerFullName })
            .Select(g => new WorkerUtilizationDto
            {
                WorkerId = g.Key.WorkerId!.Value,
                WorkerFullName = g.Key.WorkerFullName,
                TotalTasks = g.Count(),
                TotalItems = g.Sum(x => x.Quantity)
            })
            .ToList();
    }

    public async Task<List<OrderItemDto>> GetEquipmentDetailsAsync(int equipmentId)
    {
        var items = await _orderItemRepository.GetAllWithDetailsAsync();
        var filtered = items.Where(x => x.EquipmentId == equipmentId).ToList();
        return _mapper.Map<List<OrderItemDto>>(filtered);
    }

    public async Task<List<OrderItemDto>> GetWorkerDetailsAsync(int workerId)
    {
        var items = await _orderItemRepository.GetAllWithDetailsAsync();
        var filtered = items.Where(x => x.WorkerId == workerId).ToList();
        return _mapper.Map<List<OrderItemDto>>(filtered);
    }
}