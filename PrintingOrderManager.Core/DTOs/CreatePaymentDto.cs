using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace PrintingOrderManager.Core.DTOs
{
    public class CreatePaymentDto
    {
        [Required]
        public int OrderId { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Сумма должна быть больше 0")]
        public decimal Amount { get; set; }

        [Required]
        public DateOnly PaymentDate { get; set; }

        public string? PaymentStatus { get; set; }
    }
}
