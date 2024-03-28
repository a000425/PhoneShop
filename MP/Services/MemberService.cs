using AutoMapper;
using MP.Models;
using MP.Services;
using MP.Repository;
using System.Security.Cryptography;
using System.Text;

namespace MP.Services
{
    public class MemberService
    {

        private readonly MailService _mailService;
        private readonly RegisterRepository _repository;
        private readonly MemberChangeRepository _MCrepository;
        public MemberService(RegisterRepository repository, MailService mailService, MemberChangeRepository MCrepository)
        {
            _repository = repository;
            _mailService = mailService;
            _MCrepository = MCrepository;
        }
        #region 註冊
        public async Task RegisterAsync(Account account)
        {
            account.Password = HashPassword(account.Password);
            account.AuthCode = _mailService.AuthCode();
            await _repository.AddAccountAsync(account);
        }
        #endregion
        #region Hash加密
        private string  HashPassword(string password)
        {
            string saltkey = "wasd";
            string saltAndpassword = string.Concat(saltkey, password);
            SHA256 sha256 = SHA256.Create();
            var Hashed = sha256.ComputeHash(Encoding.UTF8.GetBytes(saltAndpassword));
            string HashPassword = Convert.ToBase64String(Hashed);
            return HashPassword;
        }
        #endregion     
        #region 重複帳號確認
        public bool CheckAccount(string Account){
            bool result = _repository.GetAccountAsync(Account);
            return result;
        }
        #endregion
        #region Email驗證
        public async Task<bool> EmailValidateAsync(string Account, string AuthCode)
        {
            return await _repository.ValidateEmail(Account, AuthCode);

        }
        #endregion
        /*#region 舊密碼確認
        public bool CheckPassword(string OldPassword)
        {
            OldPassword = HashPassword(OldPassword);
            return _MCrepository.CheckPassword(OldPassword);
        }
        #endregion
        #region 修改密碼
        public  async Task<bool> PasswordChange(string Password, string Account)
        {
            Password = HashPassword(Password);
            bool result = await _MCrepository.PasswordChange(Password,Account);
            return result;
        }
        #endregion*/
    }
}
