using System;
using System.Collections.Generic;

namespace MP.Models;

public partial class OrderItem
{
    public int Id { get; set; }

    public int OrderId { get; set; }

    public int ItemId { get; set; }

    public int ItemNum { get; set; }

    public virtual Order Order { get; set; } = null!;
}
