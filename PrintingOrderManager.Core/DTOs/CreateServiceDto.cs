using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace PrintingOrderManager.Core.DTOs
{
    public class CreateServiceDto
    {
        [Required(ErrorMessage = "Название услуги обязательно")]
        public string ServiceName { get; set; } = null!;

        public string? Description { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Цена за единицу должна быть больше 0")]
        public decimal UnitPrice { get; set; }
    }
}
