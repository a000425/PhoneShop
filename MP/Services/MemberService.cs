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
<<<<<<< HEAD:MP/Services/RegisterService.cs
        #region 登入
        public string Login(string account,string password){
            bool result = _repository.GetAccountAsync(account);
            if(result){
                Account data = _repository.GetAccountData(account);
                if(string.IsNullOrEmpty(data.AuthCode)){
                    if(PasswordCheck(data,password)){
                        return "登入成功";
                    }
                    else{
                        return "密碼錯誤，請重新輸入";
                    }
                }
                else{
                    return "此會員帳號還沒經過Email驗證，請去收信";
                }
            }
            else{
                return "無此會員帳號，請去註冊";
            }
        }
        #endregion
        #region 取得一筆資料
        public Account GetDataByAccount(string account){
            Account data = _repository.GetAccountData(account);
            return data;
        }
        #endregion
        #region 密碼確認
        public bool PasswordCheck(Account data,string password)
        {
            if(data.Password == HashPassword(password))
            {
                return true;
            }else
            {
                return false;
            }
        }
        #endregion
        #region 密碼變更
        public async Task<string> ChangePassword(string account,string oldpassword,string newpassword)
        {
            Account data = _repository.GetAccountData(account);
            if(PasswordCheck(data,oldpassword))
            {
                newpassword = HashPassword(newpassword);
                if(await _repository.PasswordChange(account,newpassword))
                {
                    return "密碼更改成功";
                }else
                {
                    return "密碼修改失敗";
                }
                
            }else
            {
                return "舊密碼輸入錯誤";
            }
        }
        #endregion
        
        
    }
}
