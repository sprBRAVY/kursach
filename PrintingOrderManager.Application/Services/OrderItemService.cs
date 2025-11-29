// PrintingOrderManager.Application/Services/OrderItemService.cs
using AutoMapper;
using PrintingOrderManager.Core.DTOs;
using PrintingOrderManager.Core.Entities;
using PrintingOrderManager.Core.Interfaces;

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

        public async Task<OrderItemDto> GetOrderItemByIdAsync(int id)
        {
            var orderItem = await _orderItemRepository.GetByIdWithDetailsAsync(id);
            if (orderItem == null) return null;
            return _mapper.Map<OrderItemDto>(orderItem);
        }

        public async Task AddOrderItemAsync(CreateOrderItemDto orderItemDto)
        {
            var order = await _orderRepository.GetByIdAsync(orderItemDto.OrderId);
            if (order == null)
            {
                throw new ArgumentException($"Order with ID {orderItemDto.OrderId} not found.");
            }

            var service = await _serviceRepository.GetByIdAsync(orderItemDto.ServiceId);
            if (service == null)
            {
                throw new ArgumentException($"Service with ID {orderItemDto.ServiceId} not found.");
            }

            if (orderItemDto.WorkerId.HasValue)
            {
                var worker = await _workerRepository.GetByIdAsync(orderItemDto.WorkerId.Value);
                if (worker == null)
                {
                    throw new ArgumentException($"Worker with ID {orderItemDto.WorkerId} not found.");
                }
            }

            if (orderItemDto.EquipmentId.HasValue)
            {
                var equipment = await _equipmentRepository.GetByIdAsync(orderItemDto.EquipmentId.Value);
                if (equipment == null)
                {
                    throw new ArgumentException($"Equipment with ID {orderItemDto.EquipmentId} not found.");
                }
            }

            var orderItem = _mapper.Map<OrderItem>(orderItemDto);
            await _orderItemRepository.AddAsync(orderItem);
        }

        public async Task UpdateOrderItemAsync(int id, UpdateOrderItemDto orderItemDto)
        {
            var existingOrderItem = await _orderItemRepository.GetByIdAsync(id);
            if (existingOrderItem == null)
            {
                throw new KeyNotFoundException($"OrderItem with ID {id} not found.");
            }

            var order = await _orderRepository.GetByIdAsync(orderItemDto.OrderId);
            if (order == null)
            {
                throw new ArgumentException($"Order with ID {orderItemDto.OrderId} not found.");
            }

            var service = await _serviceRepository.GetByIdAsync(orderItemDto.ServiceId);
            if (service == null)
            {
                throw new ArgumentException($"Service with ID {orderItemDto.ServiceId} not found.");
            }

            if (orderItemDto.WorkerId.HasValue)
            {
                var worker = await _workerRepository.GetByIdAsync(orderItemDto.WorkerId.Value);
                if (worker == null)
                {
                    throw new ArgumentException($"Worker with ID {orderItemDto.WorkerId} not found.");
                }
            }

            if (orderItemDto.EquipmentId.HasValue)
            {
                var equipment = await _equipmentRepository.GetByIdAsync(orderItemDto.EquipmentId.Value);
                if (equipment == null)
                {
                    throw new ArgumentException($"Equipment with ID {orderItemDto.EquipmentId} not found.");
                }
            }

            _mapper.Map(orderItemDto, existingOrderItem);
            await _orderItemRepository.UpdateAsync(existingOrderItem);
        }

        public async Task DeleteOrderItemAsync(int id)
        {
            await _orderItemRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<OrderItemDto>> GetOrderItemsByOrderIdAsync(int orderId)
        {
            var orderItems = await _orderItemRepository.GetByOrderIdAsync(orderId);
            return _mapper.Map<IEnumerable<OrderItemDto>>(orderItems);
        }

        public async Task<IEnumerable<OrderItemDto>> GetOrderItemsByServiceIdAsync(int serviceId)
        {
            var orderItems = await _orderItemRepository.GetByServiceIdAsync(serviceId);
            return _mapper.Map<IEnumerable<OrderItemDto>>(orderItems);
        }
    }
}