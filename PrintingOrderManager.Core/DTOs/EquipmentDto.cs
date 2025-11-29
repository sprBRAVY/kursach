using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintingOrderManager.Core.DTOs
{
    public class EquipmentDto
    {
        public int EquipmentId { get; set; }
        public string EquipmentName { get; set; } = null!;
        public string? Model { get; set; }
        public string? Specifications { get; set; }
    }
}
