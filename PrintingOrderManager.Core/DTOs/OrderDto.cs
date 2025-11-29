using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace PrintingOrderManager.Core.DTOs
{
    public class OrderDto
    {
        public int OrderId { get; set; }
        public int ClientId { get; set; }
        public string ClientName { get; set; } = null!;
        public DateOnly PlacementDate { get; set; }
        public DateOnly? CompletionDate { get; set; }
        public string? Status { get; set; }
        public IEnumerable<OrderItemDto> OrderItems { get; set; } = new List<OrderItemDto>();
        public IEnumerable<PaymentDto> Payments { get; set; } = new List<PaymentDto>();
    }
}
