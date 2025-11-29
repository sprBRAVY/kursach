using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintingOrderManager.Core.DTOs
{
    public class ServiceDto
    {
        public int ServiceId { get; set; }
        public string ServiceName { get; set; } = null!;
        public string? Description { get; set; }
        public decimal UnitPrice { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
