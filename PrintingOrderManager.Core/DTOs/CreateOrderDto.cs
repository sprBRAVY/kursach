using PrintingOrderManager.Core.Entities;
using PrintingOrderManager.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintingOrderManager.Core.DTOs
{
    public class CreateOrderDto
    {
        [Required(ErrorMessage = "Клиент обязателен")]
        public int ClientId { get; set; }

        [Required(ErrorMessage = "Дата размещения обязательна")]
        public DateOnly PlacementDate { get; set; }

        //public DateOnly? CompletionDate { get; set; }
        //public string? Status { get; set; }

    }
}
