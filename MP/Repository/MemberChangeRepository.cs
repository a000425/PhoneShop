using System.Formats.Asn1;
using Microsoft.EntityFrameworkCore;
using MP.Models;

namespace MP.Repository
{
    public class MemberChangeRepository
    {
        /*private readonly PhoneContext _phoneContext;
        public MemberChangeRepository(PhoneContext phoneContext)
        {
            _phoneContext = phoneContext;
        }
       public async Task<bool> PasswordChange(string Password, string Account)
       {
        var result = (from a in _phoneContext.Account
                    where a.Account1 == Account
                    select a).SingleOrDefault();
        result.Password = Password;
        try
        {
            await _phoneContext.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            return false;
        }
        return true;
       }
       public bool CheckPassword(string OldPassword)
       {
            var result = (from a in _phoneContext.Account
                    where a.Password == OldPassword
                    select a).SingleOrDefault();
            if(result == null){
                return false;
            }
            else{
                return true;
            }
       }*/

    }
}
