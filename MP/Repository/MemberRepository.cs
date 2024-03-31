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
    }
}
