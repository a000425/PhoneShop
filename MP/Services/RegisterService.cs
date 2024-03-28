using AutoMapper;
using MP.Models;
using MP.Services;
using MP.Repository;
using System.Security.Cryptography;
using System.Text;

namespace MP.Services
{
    public class RegisterService
    {

        private readonly MailService _mailService;
        private readonly RegisterRepository _repository;
        public RegisterService(RegisterRepository repository, MailService mailService)
        {
            _repository = repository;
            _mailService = mailService;
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
    }
}
