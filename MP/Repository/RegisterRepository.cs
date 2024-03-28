using Microsoft.EntityFrameworkCore;
using MP.Models;

namespace MP.Repository
{
    public class RegisterRepository
    {
        private readonly PhoneContext _phoneContext;
        public RegisterRepository(PhoneContext phoneContext)
        {
            _phoneContext = phoneContext;
        }
        public async Task AddAccountAsync(Account account)
        {
            _phoneContext.Account.Add(account);
            await _phoneContext.SaveChangesAsync();
        }
        public bool GetAccountAsync(string account){
            var result = (from a in _phoneContext.Account
                          where a.Account1 == account
                          select a).SingleOrDefault();
            if(result == null){
                return false;
            }
            return true;
        }
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

    }
}
