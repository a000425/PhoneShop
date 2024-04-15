using System;
using System.Collections.Generic;

namespace MP.Models;

public partial class Order
{
    public int OrderId { get; set; }

     public string Account { get; set; } = null!;

    public int TotalPrice { get; set; }

    public DateTime OrderTime { get; set; }

    public string OrderStatus { get; set; }

    public string Address { get; set; }

    public virtual Account AccountNavigation { get; set; } = null!;

    public virtual ICollection<OrderItem> OrderItem { get; set; } = new List<OrderItem>();
}
