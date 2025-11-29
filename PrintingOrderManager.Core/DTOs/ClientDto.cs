using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintingOrderManager.Core.DTOs
{
    public class ClientDto
    {
        public int ClientId { get; set; }
        public string ClientName { get; set; } = null!;
        public string? Contacts { get; set; }
    }
}