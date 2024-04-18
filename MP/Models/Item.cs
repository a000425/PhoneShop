using System;
using System.Collections.Generic;

namespace MP.Models;

public partial class Item
{
    public int ItemId { get; set; }

    public string ItemName { get; set; } = null!;

    public string Instruction { get; set; } = null!;

    public bool? IsAvailable { get; set; }
    public DateTime CreateTime {get;set; }
    public DateTime? UpTime{get;set;}
    public DateTime? DownTime {get;set;}
    public virtual ICollection<Cart> Cart { get; set; } = new List<Cart>();

    public virtual ICollection<Format> Format { get; set; } = new List<Format>();

    public virtual ICollection<QA> QA { get; set; } = new List<QA>();
}
