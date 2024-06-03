using Microsoft.EntityFrameworkCore;
using MP.Models;

namespace MP.Repository
{
    public class MemberRepository
    {
        private readonly PhoneContext _phoneContext;
        public MemberRepository(PhoneContext phoneContext)
        {
            _phoneContext = phoneContext;
        }
        #region 新增資料
        public async Task AddAccountAsync(Account account)
        {
            account.CanUse = true;
            _phoneContext.Account.Add(account);
            await _phoneContext.SaveChangesAsync();
        }
        #endregion
        #region 查詢此資料是否存在
        public bool GetAccountAsync(string account){
            var result = (from a in _phoneContext.Account
                          where a.Account1 == account
                          select a).SingleOrDefault();
            if(result == null){
                return false;
            }
            return true;
        }
        #endregion
        #region 信箱驗證
        public async Task<bool> ValidateEmail(string Account, string AuthCode)
        {
            var result = await _phoneContext.Account.SingleOrDefaultAsync(a => a.Account1 == Account && a.AuthCode == AuthCode);
            if (result != null)
            {
                result.AuthCode = null;
                result.RegisterTime = DateTime.Now;
                try
                {
                    await _phoneContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion
        #region 取得一筆資料
        public Account GetAccountData (string account){
            var data = _phoneContext.Account.SingleOrDefault(a=>a.Account1==account);
            return data;
        }
        #endregion
        #region 更改密碼
        public async Task<bool> PasswordChange(string Account, string newpassword)
        {
            var result = await _phoneContext.Account.SingleOrDefaultAsync(a => a.Account1 == Account);
            if (result != null)
            {
                result.Password = newpassword;
                try
                {
                    await _phoneContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion
        #region 使用者權限
        public bool GetRole(string Account)
        {
            var result = (from a in _phoneContext.Account
                        where a.Account1 == Account
                        select a.IsAdmin).SingleOrDefault();
            return result;
        }
        #endregion
        #region 忘記密碼
        public bool CheckAccountByEmail(string Account,string Email){
            var result = _phoneContext.Account.SingleOrDefault(a=> a.Account1 == Account && a.Email == Email);
            if(result!= null){
                return true;
            }
            return false;
        }
        public string GetNewPassword(string Email,string Account){
            string[] Code = { "A","B","C","D","E","F","G","H","I","J","K","L","M","N",
                              "P","Q","R","S","T","U","V","W","X","Y","Z","a","b","c",
                              "d","e","f","g","h","i","j","k","l","m","n","p","q","r",
                              "s","t","u","v","w","x","y","z","1","2","3","4","5","6","7","8","9"};
            string NewPassword = string.Empty;
            Random rd = new Random();
            for(int i=0;i<10;i++)
            {
                NewPassword += Code[rd.Next(Code.Count())];
            }
            return NewPassword;
        }
        public void ForgetPasswodChange(string Account, string Password){
            try{
                var result = _phoneContext.Account.Single(a=> a.Account1 == Account);
                if(result != null){
                    result.Password = Password;
                    _phoneContext.SaveChanges();
                }
            }
            catch(Exception ex){
                throw new Exception(ex.ToString());
            }
        }
        #endregion
        #region 會員等級檢查
        public bool MemberKindCheck(string Account)
        {
            var member = _phoneContext.Account.SingleOrDefault(a=> a.Account1 == Account);
            var memberTime = member.MemberTime;
            var EXPTime = DateTime.Now.AddYears(-1);
            if(memberTime < EXPTime)
            {
                member.MemberKind = null;
                member.MemberTime = null;
                _phoneContext.SaveChanges();
                return true;
            }
            return false;
        }
        #endregion
        #region 會員停權確認
        public bool MemberCanUseCheck(string Account)
        {
            try{
                var result = _phoneContext.Account.Single(a=> a.Account1 == Account);
                if(result.CanUse == false){
                    return false;
                }else
                {
                    return true;
                }
            }
            catch(Exception ex){
                throw new Exception(ex.ToString());
            }
            
            
        }
        #endregion
    }
}
