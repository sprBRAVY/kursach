// PrintingOrderManager.Application.Services/IEquipmentService.cs
using PrintingOrderManager.Core.DTOs;
using System.Linq;

namespace PrintingOrderManager.Application.Services
{
    public interface IEquipmentService
    {
        Task<IEnumerable<EquipmentDto>> GetAllEquipmentAsync();
        IQueryable<EquipmentDto> GetEquipmentQueryable(); // ← ДОБАВЛЕНО
        Task<EquipmentDto> GetEquipmentByIdAsync(int id);
        Task AddEquipmentAsync(CreateEquipmentDto equipmentDto);
        Task UpdateEquipmentAsync(int id, UpdateEquipmentDto equipmentDto);
        Task DeleteEquipmentAsync(int id);
        Task<EquipmentDto?> GetEquipmentByNameAsync(string name);
        Task<EquipmentDto?> GetEquipmentByModelAsync(string model);
        Task<IEnumerable<OrderItemDto>> GetOrderItemsByEquipmentIdAsync(int equipmentId);
    }
}