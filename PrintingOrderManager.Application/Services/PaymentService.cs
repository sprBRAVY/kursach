// PrintingOrderManager.Application/Services/PaymentService.cs
using AutoMapper;
using PrintingOrderManager.Core.DTOs;
using PrintingOrderManager.Core.Entities;
using PrintingOrderManager.Core.Interfaces;

namespace PrintingOrderManager.Application.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public PaymentService(IPaymentRepository paymentRepository, IOrderRepository orderRepository, IMapper mapper)
        {
            _paymentRepository = paymentRepository;
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PaymentDto>> GetAllPaymentsAsync()
        {
            var payments = await _paymentRepository.GetAllWithOrderAsync();
            return _mapper.Map<IEnumerable<PaymentDto>>(payments);
        }

        public async Task<PaymentDto> GetPaymentByIdAsync(int id)
        {
            var payment = await _paymentRepository.GetByIdWithOrderAsync(id);
            if (payment == null) return null;
            return _mapper.Map<PaymentDto>(payment);
        }

        public async Task AddPaymentAsync(CreatePaymentDto paymentDto)
        {
            var order = await _orderRepository.GetByIdAsync(paymentDto.OrderId);
            if (order == null)
            {
                throw new ArgumentException($"Order with ID {paymentDto.OrderId} not found.");
            }

            var payment = _mapper.Map<Payment>(paymentDto);
            await _paymentRepository.AddAsync(payment);
        }

        public async Task UpdatePaymentAsync(int id, UpdatePaymentDto paymentDto)
        {
            var existingPayment = await _paymentRepository.GetByIdAsync(id);
            if (existingPayment == null)
            {
                throw new KeyNotFoundException($"Payment with ID {id} not found.");
            }

            var order = await _orderRepository.GetByIdAsync(paymentDto.OrderId);
            if (order == null)
            {
                throw new ArgumentException($"Order with ID {paymentDto.OrderId} not found.");
            }

            _mapper.Map(paymentDto, existingPayment);
            await _paymentRepository.UpdateAsync(existingPayment);
        }

        public async Task DeletePaymentAsync(int id)
        {
            await _paymentRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<PaymentDto>> GetPaymentsByOrderIdAsync(int orderId)
        {
            var payments = await _paymentRepository.GetByOrderIdAsync(orderId);
            return _mapper.Map<IEnumerable<PaymentDto>>(payments);
        }

        public async Task<IEnumerable<PaymentDto>> GetPaymentsByAmountRangeAsync(decimal minAmount, decimal maxAmount)
        {
            var payments = await _paymentRepository.GetAllWithOrderAsync();
            var filteredPayments = payments.Where(p => p.Amount >= minAmount && p.Amount <= maxAmount);
            return _mapper.Map<IEnumerable<PaymentDto>>(filteredPayments);
        }
    }
}