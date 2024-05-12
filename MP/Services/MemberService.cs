using AutoMapper;
using MP.Models;
using MP.Services;
using MP.Repository;
using System.Security.Cryptography;
using System.Text;
using Azure;
using Microsoft.IdentityModel.Tokens;
using MP.Dtos;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MP.Services
{
    public class MemberService
    {
        private readonly MailService _mailService;
        private readonly MemberRepository _repository;
        private readonly IConfiguration _configuration;
        public MemberService(MemberRepository repository,MailService mailService, IConfiguration configuration)
        {
            _repository = repository;
            _mailService = mailService;
            _configuration = configuration;
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
        #region 取得一筆資料
        public Account GetDataByAccount(string account){
            Account data = _repository.GetAccountData(account);
            return data;
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
        #region 登入
        public string Login(string account,string password){
            bool result = _repository.GetAccountAsync(account);
            if(result){
                Account data = _repository.GetAccountData(account);
                if(string.IsNullOrEmpty(data.AuthCode)){
                    if(PasswordCheck(data,password)){
                        if(_repository.MemberKindCheck(account))
                        {
                            return "登入成功，會員等級已變更";
                        }
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
        #region 使用者權限
        public string IsAdmin(string Account)
        {
            if(_repository.GetRole(Account)){
                return "Admin";
            }
            return "User";
        }
        #endregion
        #region 產生權限
        public string GenerateToken(LoginDto loginDto)
        {
            var claims = new []
            {
                new Claim(JwtRegisteredClaimNames.NameId, loginDto.Account1),
                new Claim(ClaimTypes.Name, loginDto.Account1),
                new Claim(ClaimTypes.Role,IsAdmin(loginDto.Account1))
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:KEY"]));

            var jwt = new JwtSecurityToken
            (
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256)
            );
            string token = new JwtSecurityTokenHandler().WriteToken(jwt).ToString();
            return token;
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
        #region 忘記密碼
        public string ForgetPasswod(string Email, string Account){
            if(_repository.CheckAccountByEmail(Account, Email)){
                var result = _repository.GetNewPassword(Email,Account);
                var password = HashPassword(result);
                _repository.ForgetPasswodChange(Account, password);
                return result;
            }
            return "";
        }
        #endregion
        
    }
}
