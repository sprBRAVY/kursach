using System;
using System.Collections.Generic;

namespace PrintingOrderManager.Core.Entities;

public partial class Worker
{
    public int WorkerId { get; set; }

    public string WorkerFullName { get; set; } = null!;

    public string? Position { get; set; }

    public string? Contacts { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
