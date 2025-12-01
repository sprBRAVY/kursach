using System;
using System.Collections.Generic;

namespace PrintingOrderManager.Core.Entities;

public partial class Order
{
    public int OrderId { get; set; }

    public int ClientId { get; set; }

    public DateOnly PlacementDate { get; set; }

    public DateOnly? CompletionDate { get; set; }

    public string? Status { get; set; } = "Новый";

    public virtual Client Client { get; set; } = null!;

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
