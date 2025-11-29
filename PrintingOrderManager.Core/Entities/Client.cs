using System;
using System.Collections.Generic;



namespace PrintingOrderManager.Core.Entities;

public partial class Client
{
    public int ClientId { get; set; }

    public string ClientName { get; set; } = null!;

    public string? Contacts { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
