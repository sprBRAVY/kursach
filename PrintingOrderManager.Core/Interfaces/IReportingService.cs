using PrintingOrderManager.Core.DTOs;

public interface IReportingService
{
    // Загрузка оборудования
    Task<List<EquipmentUtilizationDto>> GetEquipmentUtilizationAsync();

    // Загрузка сотрудников
    Task<List<WorkerUtilizationDto>> GetWorkerUtilizationAsync();

    // Детали по оборудованию
    Task<List<OrderItemDto>> GetEquipmentDetailsAsync(int equipmentId);

    // Детали по сотруднику
    Task<List<OrderItemDto>> GetWorkerDetailsAsync(int workerId);
}