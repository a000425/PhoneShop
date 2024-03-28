using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MP.Models;

public partial class Account
{
    [Required(ErrorMessage ="請輸入帳號")]
    [StringLength(20,ErrorMessage ="不得超過20字元")]
    public string Account1 { get; set; } = null!;
    [Required(ErrorMessage = "請輸入密碼")]
    public string Password { get; set; } = null!;
    [Required(ErrorMessage = "請輸入姓名")]
    [StringLength(10, ErrorMessage = "不得超過10字元")]
    public string Name { get; set; } = null!;
    [Required(ErrorMessage = "請輸入電子信箱")]
    [StringLength(50, ErrorMessage = "不得超過50字元")]
    public string Email { get; set; } = null!;
    [Required(ErrorMessage = "請輸入電話號碼")]
    [StringLength(10, ErrorMessage = "不得超過10字元")]
    public string Cellphone { get; set; } = null!;
    [MaxLength(50)]
    public string? AuthCode { get; set; }

    public bool IsAdmin { get; set; }

    public virtual ICollection<Cart> Cart { get; set; } = new List<Cart>();

    public virtual ICollection<Order> Order { get; set; } = new List<Order>();

    public virtual ICollection<QA> QA { get; set; } = new List<QA>();
}
