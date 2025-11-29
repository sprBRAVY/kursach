using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace PrintingOrderManager.Core.DTOs
{
    public class UpdateEquipmentDto
    {
        [Required]
        public int EquipmentId { get; set; }

        [Required(ErrorMessage = "Название оборудования обязательно")]
        public string EquipmentName { get; set; } = null!;

        public string? Model { get; set; }
        public string? Specifications { get; set; }
    }
}
