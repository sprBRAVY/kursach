using System;
using System.Collections.Generic;

namespace PrintingOrderManager.Core.Entities;

public partial class OrderItem
{
    public int ItemId { get; set; }

    public int OrderId { get; set; }

    public int ServiceId { get; set; }

    public int? WorkerId { get; set; }

    public int? EquipmentId { get; set; }

    public string? Paper { get; set; }

    public string? Color { get; set; }

    public int Quantity { get; set; }

    public decimal Cost { get; set; }

    public virtual Equipment? Equipment { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual Service Service { get; set; } = null!;

    public virtual Worker? Worker { get; set; }
}
