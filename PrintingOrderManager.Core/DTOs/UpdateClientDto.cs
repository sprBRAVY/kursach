using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace PrintingOrderManager.Core.DTOs
{
    public class UpdateClientDto
    {
        [Required]
        public int ClientId { get; set; }

        [Required(ErrorMessage = "Имя клиента обязательно")]
        [StringLength(100, ErrorMessage = "Имя клиента не должно превышать 100 символов")]
        public string ClientName { get; set; } = null!;

        public string? Contacts { get; set; }
    }
}