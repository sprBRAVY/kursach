using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintingOrderManager.Core.DTOs
{
    public class OrderItemDto
    {
        public int ItemId { get; set; }
        public int OrderId { get; set; }
        public int ServiceId { get; set; }
        public string ServiceName { get; set; } = null!;
        public int? WorkerId { get; set; }
        public string? WorkerFullName { get; set; }
        public int? EquipmentId { get; set; }
        public string? EquipmentName { get; set; }
        public string? Paper { get; set; }
        public string? Color { get; set; }
        public int Quantity { get; set; }
        public decimal Cost { get; set; }
    }
}
