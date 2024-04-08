using System;
using System.Collections.Generic;

namespace MP.Models;

public partial class Account
{
    public string Account1 { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Cellphone { get; set; }

    public string? AuthCode { get; set; }

    public bool IsAdmin { get; set; }

    public virtual ICollection<Cart> Cart { get; set; } = new List<Cart>();

    public virtual ICollection<Order> Order { get; set; } = new List<Order>();

    public virtual ICollection<QA> QA { get; set; } = new List<QA>();
}
