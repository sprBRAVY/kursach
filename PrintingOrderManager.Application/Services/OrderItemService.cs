// PrintingOrderManager.Application/Services/OrderItemService.cs
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using PrintingOrderManager.Core.DTOs;
using PrintingOrderManager.Core.Entities;
using PrintingOrderManager.Core.Interfaces;
using System.Linq;

namespace PrintingOrderManager.Application.Services
{
    public class OrderItemService : IOrderItemService
    {
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IServiceRepository _serviceRepository;
        private readonly IWorkerRepository _workerRepository;
        private readonly IEquipmentRepository _equipmentRepository;
        private readonly IMapper _mapper;

        public OrderItemService(
            IOrderItemRepository orderItemRepository,
            IOrderRepository orderRepository,
            IServiceRepository serviceRepository,
            IWorkerRepository workerRepository,
            IEquipmentRepository equipmentRepository,
            IMapper mapper)
        {
            _orderItemRepository = orderItemRepository;
            _orderRepository = orderRepository;
            _serviceRepository = serviceRepository;
            _workerRepository = workerRepository;
            _equipmentRepository = equipmentRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<OrderItemDto>> GetAllOrderItemsAsync()
        {
            var orderItems = await _orderItemRepository.GetAllWithDetailsAsync();
            return _mapper.Map<IEnumerable<OrderItemDto>>(orderItems);
        }

        public IQueryable<OrderItemDto> GetOrderItemsQueryable()
        {
            return _orderItemRepository.GetQueryable()
                .Include(oi => oi.Service)
                .Include(oi => oi.Worker)
                .Include(oi => oi.Equipment)
                .Include(oi => oi.Order.Client)
                .ProjectTo<OrderItemDto>(_mapper.ConfigurationProvider);
        }

        public async Task<OrderItemDto> GetOrderItemByIdAsync(int id)
        {
            var orderItem = await _orderItemRepository.GetByIdWithDetailsAsync(id);
            return orderItem == null ? null : _mapper.Map<OrderItemDto>(orderItem);
        }

        public async Task AddOrderItemAsync(CreateOrderItemDto orderItemDto)
        {
            // ... (оставьте существующую валидацию зависимостей без изменений)
            var order = await _orderRepository.GetByIdAsync(orderItemDto.OrderId);
            if (order == null) throw new ArgumentException($"Order ID {orderItemDto.OrderId} not found.");
            var service = await _serviceRepository.GetByIdAsync(orderItemDto.ServiceId);
            if (service == null) throw new ArgumentException($"Service ID {orderItemDto.ServiceId} not found.");
            if (orderItemDto.WorkerId.HasValue)
            {
                var worker = await _workerRepository.GetByIdAsync(orderItemDto.WorkerId.Value);
                if (worker == null) throw new ArgumentException($"Worker ID {orderItemDto.WorkerId} not found.");
            }
            if (orderItemDto.EquipmentId.HasValue)
            {
                var equipment = await _equipmentRepository.GetByIdAsync(orderItemDto.EquipmentId.Value);
                if (equipment == null) throw new ArgumentException($"Equipment ID {orderItemDto.EquipmentId} not found.");
            }      
            var cost = await CalculateCostAsync(orderItemDto); // ← РАСЧЁТ

            var orderItem = _mapper.Map<OrderItem>(orderItemDto);
            orderItem.Cost = cost; // ← УСТАНОВКА

            await _orderItemRepository.AddAsync(orderItem);
        }

        public async Task UpdateOrderItemAsync(int id, UpdateOrderItemDto orderItemDto)
        {
            // ... (оставьте существующую валидацию зависимостей без изменений)
            var existing = await _orderItemRepository.GetByIdAsync(id);
            if (existing == null) throw new KeyNotFoundException($"OrderItem ID {id} not found.");
            // Валидация зависимостей...
            var cost = await CalculateCostAsync(orderItemDto); // ← РАСЧЁТ

            _mapper.Map(orderItemDto, existing);
            existing.Cost = cost; // ← ОБНОВЛЕНИЕ

            await _orderItemRepository.UpdateAsync(existing);    
        }

        public async Task DeleteOrderItemAsync(int id)
        {
            await _orderItemRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<OrderItemDto>> GetOrderItemsByOrderIdAsync(int orderId)
        {
            var items = await _orderItemRepository.GetByOrderIdAsync(orderId);
            return _mapper.Map<IEnumerable<OrderItemDto>>(items);
        }

        public async Task<IEnumerable<OrderItemDto>> GetOrderItemsByServiceIdAsync(int serviceId)
        {
            var items = await _orderItemRepository.GetByServiceIdAsync(serviceId);
            return _mapper.Map<IEnumerable<OrderItemDto>>(items);
        }


        public async Task UpdateOrderItemStatusAsync(int itemId, string newStatus)
        {
            var item = await _orderItemRepository.GetByIdAsync(itemId);
            if (item == null)
                throw new KeyNotFoundException($"OrderItem with ID {itemId} not found.");

            item.Status = newStatus;
            await _orderItemRepository.UpdateAsync(item);

            // Автоматически обновить статус заказа
            await UpdateOrderStatusAsync(item.OrderId);
        }

        private async Task UpdateOrderStatusAsync(int orderId)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            var items = await _orderItemRepository.GetByOrderIdAsync(orderId);

            if (items.All(i => i.Status == "Готово"))
            {
                order.Status = "Выполнен";
                order.CompletionDate = DateOnly.FromDateTime(DateTime.Now);
            }
            else if (items.Any(i => i.Status == "В работе"))
            {
                order.Status = "В процессе";
            }
            // Иначе остаётся "Новый"

            await _orderRepository.UpdateAsync(order);
        }

        private async Task<decimal> CalculateCostAsync(CreateOrderItemDto dto)
        {
            var service = await _serviceRepository.GetByIdAsync(dto.ServiceId);
            if (service == null)
                throw new ArgumentException($"Service with ID {dto.ServiceId} not found.");

            decimal baseCost = service.UnitPrice * dto.Quantity;

            // Наценка за бумагу
            decimal paperCost = dto.Paper switch
            {
                "Глянцевая" => 5m,
                "Матовая" => 4m,
                _ => 3m // Обычная
            };

            // Наценка за цвет
            decimal colorCost = dto.Color switch
            {
                "Цветная" => 2m,
                _ => 0m // Черно-белая
            };

            return baseCost + (paperCost + colorCost) * dto.Quantity;
        }

        private async Task<decimal> CalculateCostAsync(UpdateOrderItemDto dto)
        {
            var service = await _serviceRepository.GetByIdAsync(dto.ServiceId);
            if (service == null)
                throw new ArgumentException($"Service with ID {dto.ServiceId} not found.");

            decimal baseCost = service.UnitPrice * dto.Quantity;

            decimal paperCost = dto.Paper switch
            {
                "Глянцевая" => 5m,
                "Матовая" => 4m,
                _ => 3m
            };

            decimal colorCost = dto.Color switch
            {
                "Цветная" => 2m,
                _ => 0m
            };

            return baseCost + (paperCost + colorCost) * dto.Quantity;
        }
    }
}