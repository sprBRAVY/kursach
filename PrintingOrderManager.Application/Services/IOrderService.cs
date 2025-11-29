// PrintingOrderManager.Application/Services/IOrderService.cs
using PrintingOrderManager.Core.DTOs;

namespace PrintingOrderManager.Application.Services
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderDto>> GetAllOrdersAsync();
        Task<OrderDto> GetOrderByIdAsync(int id);
        Task AddOrderAsync(CreateOrderDto orderDto);
        Task UpdateOrderAsync(int id, UpdateOrderDto orderDto);
        Task DeleteOrderAsync(int id);
        Task<IEnumerable<OrderDto>> GetOrdersByClientIdAsync(int clientId);
        Task<IEnumerable<OrderDto>> GetOrdersByStatusAsync(string status);
        Task<IEnumerable<OrderDto>> GetOrdersByPlacementDateAsync(DateOnly placementDate);
    }
}