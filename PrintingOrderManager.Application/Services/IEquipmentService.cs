// PrintingOrderManager.Application/Services/IEquipmentService.cs
using PrintingOrderManager.Core.DTOs;

namespace PrintingOrderManager.Application.Services
{
    public interface IEquipmentService
    {
        Task<IEnumerable<EquipmentDto>> GetAllEquipmentAsync();
        Task<EquipmentDto> GetEquipmentByIdAsync(int id);
        Task AddEquipmentAsync(CreateEquipmentDto equipmentDto);
        Task UpdateEquipmentAsync(int id, UpdateEquipmentDto equipmentDto);
        Task DeleteEquipmentAsync(int id);
        Task<EquipmentDto?> GetEquipmentByNameAsync(string name);
        Task<EquipmentDto?> GetEquipmentByModelAsync(string model);
    }
}