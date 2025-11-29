using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintingOrderManager.Core.DTOs
{
    public class WorkerDto
    {
        public int WorkerId { get; set; }
        public string WorkerFullName { get; set; } = null!;
        public string? Position { get; set; }
        public string? Contacts { get; set; }
    }
}
