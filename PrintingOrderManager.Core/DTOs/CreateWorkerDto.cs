using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace PrintingOrderManager.Core.DTOs
{
    public class CreateWorkerDto
    {
        [Required(ErrorMessage = "ФИО работника обязательно")]
        public string WorkerFullName { get; set; } = null!;

        public string? Position { get; set; }
        public string? Contacts { get; set; }
    }
}
