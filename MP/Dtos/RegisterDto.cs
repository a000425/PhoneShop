using System.ComponentModel.DataAnnotations;

namespace MP.Dtos
{
    public class RegisterDto
    {

        [Required(ErrorMessage = "請輸入帳號")]
        [StringLength(20, ErrorMessage = "不得超過20字元")]
        public string Account1 { get; set; } = null!;
        [Required(ErrorMessage = "請輸入密碼")]
        public string Password { get; set; } = null!;
        [Required(ErrorMessage = "請輸入姓名")]
        [StringLength(10, ErrorMessage = "不得超過10字元")]
        public string Name { get; set; } = null!;
        [Required(ErrorMessage = "請輸入電話號碼")]
        [StringLength(10, ErrorMessage = "不得超過10字元")]
        public string Cellphone { get; set; } = null!;
        [Required(ErrorMessage = "請輸入電子信箱")]
        [StringLength(50, ErrorMessage = "不得超過50字元")]
        public string Email { get; set; } = null!;
    }
}
