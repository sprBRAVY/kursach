using System;
using System.Collections.Generic;

namespace PrintingOrderManager.Core.Entities;

public partial class Equipment
{
    public int EquipmentId { get; set; }

    public string EquipmentName { get; set; } = null!;

    public string? Model { get; set; }

    public string? Specifications { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
