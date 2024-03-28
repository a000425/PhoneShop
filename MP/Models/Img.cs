using System;
using System.Collections.Generic;

namespace MP.Models;

public partial class Img
{
    public int Id { get; set; }

    public int FormatId { get; set; }

    public string? ItemImg { get; set; }

    public virtual Format Format { get; set; } = null!;
}
