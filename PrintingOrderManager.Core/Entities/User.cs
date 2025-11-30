using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintingOrderManager.Core.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public string Role { get; set; } = "User"; // "User" или "Admin"
    }
}
