using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace MP.Dtos
{
    public class PasswordChangeDto
    {
        [DisplayName("舊密碼")]
        [Required(ErrorMessage ="請輸入舊密碼")]
        public string OldPassword{get;set;} = null!;
        [DisplayName("新密碼")]
        [Required(ErrorMessage ="請輸入新密碼")]
        public string Password{get;set;} = null!;
        [DisplayName("密碼確認")]
        [Required(ErrorMessage ="請再輸入一次密碼")]
        [Compare("Password",ErrorMessage ="兩次密碼輸入不一致")]
        public string PasswordCheck{get;set;} = null!;
    }
}
