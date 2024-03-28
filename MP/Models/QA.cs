using System;
using System.Collections.Generic;

namespace MP.Models;

public partial class QA
{
    public int Id { get; set; }

    public int ItemId { get; set; }

    public string Account { get; set; } = null!;

    public string Content { get; set; } = null!;

    public DateTime CreateTime { get; set; }

    public string? Reply { get; set; }

    public DateTime? ReplyTime { get; set; }

    public virtual Account AccountNavigation { get; set; } = null!;

    public virtual Item Item { get; set; } = null!;
}
