// PrintingOrderManager.Application/Services/IServiceService.cs
using PrintingOrderManager.Core.DTOs;

namespace PrintingOrderManager.Application.Services
{
    public interface IServiceService
    {
        Task<IEnumerable<ServiceDto>> GetAllServicesAsync();
        Task<ServiceDto> GetServiceByIdAsync(int id);
        Task AddServiceAsync(CreateServiceDto serviceDto);
        Task UpdateServiceAsync(int id, UpdateServiceDto serviceDto);
        Task DeleteServiceAsync(int id);
        Task<ServiceDto?> GetServiceByNameAsync(string name);
        Task<IEnumerable<ServiceDto>> GetServicesByPriceRangeAsync(decimal minPrice, decimal maxPrice);
    }
}