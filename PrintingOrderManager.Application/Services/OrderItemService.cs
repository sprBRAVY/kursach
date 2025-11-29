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
            var orderItem = _mapper.Map<OrderItem>(orderItemDto);
            await _orderItemRepository.AddAsync(orderItem);
        }

        public async Task UpdateOrderItemAsync(int id, UpdateOrderItemDto orderItemDto)
        {
            // ... (оставьте существующую валидацию зависимостей без изменений)
            var existing = await _orderItemRepository.GetByIdAsync(id);
            if (existing == null) throw new KeyNotFoundException($"OrderItem ID {id} not found.");
            // Валидация зависимостей...
            _mapper.Map(orderItemDto, existing);
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
    }
}