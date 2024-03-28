using AutoMapper;
using MP.Dtos;
using MP.Models;

namespace MP.Profiles
{
    public class MPProfile:Profile
    {
        public MPProfile()
        {
            CreateMap<Account, RegisterDto>();
            CreateMap<Account, LoginDto>();
        }
    }
}
