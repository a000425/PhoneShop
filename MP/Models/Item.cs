using System;
using System.Collections.Generic;

namespace MP.Models;

public partial class Item
{
    public int ItemId { get; set; }

    public string ItemName { get; set; } = null!;

    public int ItemPrice { get; set; }

    public bool IsAvailable { get; set; }

    public int FormatId { get; set; }

    public virtual ICollection<Cart> Cart { get; set; } = new List<Cart>();

    public virtual Format Format { get; set; } = null!;

    public virtual ICollection<QA> QA { get; set; } = new List<QA>();
}
