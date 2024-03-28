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
    }
}
