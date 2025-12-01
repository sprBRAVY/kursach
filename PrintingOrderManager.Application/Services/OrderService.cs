// PrintingOrderManager.Application.Services/OrderService.cs
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using PrintingOrderManager.Core.DTOs;
using PrintingOrderManager.Core.Entities;
using PrintingOrderManager.Core.Interfaces;
using System.Linq;

namespace PrintingOrderManager.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IMapper _mapper;

        public OrderService(IOrderRepository orderRepository, IClientRepository clientRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _clientRepository = clientRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<OrderDto>> GetAllOrdersAsync()
        {
            var orders = await _orderRepository.GetAllWithDetailsAsync();
            return _mapper.Map<IEnumerable<OrderDto>>(orders);
        }

        public IQueryable<OrderDto> GetOrdersQueryable()
        {
            return _orderRepository.GetQueryable()
                .Include(o => o.Client)
                .ProjectTo<OrderDto>(_mapper.ConfigurationProvider);
        }

        public async Task<OrderDto> GetOrderByIdAsync(int id)
        {
            var order = await _orderRepository.GetByIdWithDetailsAsync(id);
            if (order == null) return null;

            var dto = _mapper.Map<OrderDto>(order);

            // Расчёт итогов
            dto.TotalCost = order.OrderItems.Sum(oi => oi.Cost);
            dto.PaidAmount = order.Payments.Where(p => p.PaymentStatus == "Оплачено").Sum(p => p.Amount);
            dto.IsPaid = dto.PaidAmount >= dto.TotalCost;

            return dto;
        }

        public async Task AddOrderAsync(CreateOrderDto orderDto)
        {
            var client = await _clientRepository.GetByIdAsync(orderDto.ClientId);
            if (client == null)
                throw new ArgumentException($"Client with ID {orderDto.ClientId} not found.");

            var order = _mapper.Map<Order>(orderDto);
            order.Status = "Новый"; // ← ЯВНО УСТАНАВЛИВАЕМ СТАТУС ПРИ СОЗДАНИИ
            await _orderRepository.AddAsync(order);
        }

        public async Task UpdateOrderAsync(int id, UpdateOrderDto orderDto)
        {
            var existingOrder = await _orderRepository.GetByIdAsync(id);
            if (existingOrder == null)
                throw new KeyNotFoundException($"Order with ID {id} not found.");
            var client = await _clientRepository.GetByIdAsync(orderDto.ClientId);
            if (client == null)
                throw new ArgumentException($"Client with ID {orderDto.ClientId} not found.");
            _mapper.Map(orderDto, existingOrder);
            await _orderRepository.UpdateAsync(existingOrder);
        }

        public async Task DeleteOrderAsync(int id)
        {
            await _orderRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<OrderDto>> GetOrdersByClientIdAsync(int clientId)
        {
            var orders = await _orderRepository.GetByClientIdAsync(clientId);
            return _mapper.Map<IEnumerable<OrderDto>>(orders);
        }

        public async Task<IEnumerable<OrderDto>> GetOrdersByStatusAsync(string status)
        {
            var orders = await _orderRepository.GetByStatusAsync(status);
            return _mapper.Map<IEnumerable<OrderDto>>(orders);
        }

        public async Task<IEnumerable<OrderDto>> GetOrdersByPlacementDateAsync(DateOnly placementDate)
        {
            var orders = await _orderRepository.GetBetweenDatesAsync(placementDate, placementDate);
            return _mapper.Map<IEnumerable<OrderDto>>(orders);
        }
    }
}