// PrintingOrderManager.Application/Services/IOrderItemService.cs
using PrintingOrderManager.Core.DTOs;
using System.Linq;

namespace PrintingOrderManager.Application.Services
{
    public interface IOrderItemService
    {
        Task<IEnumerable<OrderItemDto>> GetAllOrderItemsAsync();
        IQueryable<OrderItemDto> GetOrderItemsQueryable(); // ← НОВЫЙ МЕТОД
        Task<OrderItemDto> GetOrderItemByIdAsync(int id);
        Task AddOrderItemAsync(CreateOrderItemDto orderItemDto);
        Task UpdateOrderItemAsync(int id, UpdateOrderItemDto orderItemDto);
        Task DeleteOrderItemAsync(int id);
        Task<IEnumerable<OrderItemDto>> GetOrderItemsByOrderIdAsync(int orderId);
        Task<IEnumerable<OrderItemDto>> GetOrderItemsByServiceIdAsync(int serviceId);
        Task UpdateOrderItemStatusAsync(int itemId, string newStatus);
    }
}