using System.ComponentModel.DataAnnotations;

namespace MP.Dtos
{
    public class ChangePasswordDto
    {
        
        [Required(ErrorMessage ="請輸入舊密碼")]
        public string OldPassword{get;set;}
        [Required(ErrorMessage ="請輸入新密碼")]
        public string NewPassword{get;set;}
        [Required(ErrorMessage ="請再次輸入新密碼")]
        [Compare("NewPassword",ErrorMessage ="兩次密碼輸入不一致")]
        public string NewPasswordCheck{get;set;}
    }
}