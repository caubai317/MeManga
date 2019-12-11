using AutoMapper;
using MeManga.Core.Business.Models.Users;
using MeManga.Core.Entities;

namespace MeManga.Core.Business.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserRegisterModel>().ReverseMap();
        }
    }
}
