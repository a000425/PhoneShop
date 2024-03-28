using System.ComponentModel.DataAnnotations;

namespace MP.Dtos
{
    public class LoginDto
    {
        [Required(ErrorMessage = "請輸入帳號")]
        [StringLength(20, ErrorMessage = "不得超過20字元")]
        public string Account1 { get; set; } = null!;
        [Required(ErrorMessage = "請輸入密碼")]
        public string Password { get; set; } = null!;
    }
}
