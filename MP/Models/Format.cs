using System;
using System.Collections.Generic;

namespace MP.Models;

public partial class Format
{
    public int FormatId { get; set; }

    public string Brand { get; set; } = null!;

    public string Color { get; set; }

    public int ItemPrice { get; set; }

    public int Store { get; set; }

    public string Space { get; set; } = null!;

    public int ItemId { get; set; }

    public virtual ICollection<Img> Img { get; set; } = new List<Img>();

    public virtual Item Item { get; set; } = null!;
}
