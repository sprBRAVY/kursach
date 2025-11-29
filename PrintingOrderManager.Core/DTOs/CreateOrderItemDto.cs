using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace PrintingOrderManager.Core.DTOs
{
    public class CreateOrderItemDto
    {
        [Required]
        public int OrderId { get; set; }

        [Required]
        public int ServiceId { get; set; }

        public int? WorkerId { get; set; }
        public int? EquipmentId { get; set; }

        public string? Paper { get; set; }
        public string? Color { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Количество должно быть больше 0")]
        public int Quantity { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Стоимость должна быть больше 0")]
        public decimal Cost { get; set; }
    }
}
