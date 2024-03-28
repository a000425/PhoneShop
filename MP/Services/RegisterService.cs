using AutoMapper;
using MP.Models;
using MP.Repository;
using System.Security.Cryptography;
using System.Text;

namespace MP.Services
{
    public class RegisterService
    {

        private readonly RegisterRepository _repository;
        public RegisterService(RegisterRepository repository)
        {
            _repository = repository;
        }
        #region 註冊
        public async Task RegisterAsync(Account account)
        {
            account.Password = HashPassword(account.Password);
            account.AuthCode = AuthCode();
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

        #region 產生驗證碼
        public string AuthCode()
        {
            string[] Code = { "A","B","C","D","E","F","G","H","I","J","K","L","M","N",
                              "P","Q","R","S","T","U","V","W","X","Y","Z","a","b","c",
                              "d","e","f","g","h","i","j","k","l","m","n","p","q","r",
                              "s","t","u","v","w","x","y","z","1","2","3","4","5","6","7","8","9"};
            string ValidateCode = string.Empty;
            Random rd = new Random();
            for(int i=0;i<10;i++)
            {
                ValidateCode += Code[rd.Next(Code.Count())];
            }
            return ValidateCode;
        }
        #endregion
        #region 產生驗證信
        public string GetMailBody(string Temp,string account,string ValidatrUrl)
        {
            Temp = Temp.Replace("{{account}}", account);
            Temp = Temp.Replace("{{ValidateUrl}}", ValidatrUrl);
            return Temp;
        }
        #endregion
        #region 寄送驗證信

        #endregion
        
        #region 重複帳號確認

        #endregion
    }
}
