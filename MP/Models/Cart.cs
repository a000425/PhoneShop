using System;
using System.Collections.Generic;

namespace MP.Models;

public partial class Cart
{
    public int Id { get; set; }

    public int CartId { get; set; }

    public string Account { get; set; } = null!;

    public int ItemId { get; set; }

    public int ItemNum { get; set; }

    public DateTime AddTime { get; set; }

    public virtual Account AccountNavigation { get; set; } = null!;

    public virtual Item Item { get; set; } = null!;
}
