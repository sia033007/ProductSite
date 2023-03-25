using AutoMapper;

namespace ShopSite.Models
{
    public class UserMapper : Profile
    {
        public UserMapper()
        {
            CreateMap<RegisterModel, MyUser>().ReverseMap();
        }
    }
}
