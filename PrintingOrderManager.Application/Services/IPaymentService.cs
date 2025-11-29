// PrintingOrderManager.Application/Services/IPaymentService.cs
using PrintingOrderManager.Core.DTOs;

namespace PrintingOrderManager.Application.Services
{
    public interface IPaymentService
    {
        Task<IEnumerable<PaymentDto>> GetAllPaymentsAsync();
        Task<PaymentDto> GetPaymentByIdAsync(int id);
        Task AddPaymentAsync(CreatePaymentDto paymentDto);
        Task UpdatePaymentAsync(int id, UpdatePaymentDto paymentDto);
        Task DeletePaymentAsync(int id);
        Task<IEnumerable<PaymentDto>> GetPaymentsByOrderIdAsync(int orderId);
        Task<IEnumerable<PaymentDto>> GetPaymentsByAmountRangeAsync(decimal minAmount, decimal maxAmount);
    }
}