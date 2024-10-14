using Amazon_eCommerce_API.Models.DTO_s;
using Amazon_eCommerce_API.Models.Users;
using AutoMapper;

namespace Amazon_eCommerce_API.Profiles
{
    public class UserMappingProfile : Profile
    {


        public UserMappingProfile() {


            CreateMap<UserRegistrationDto, User>();


            CreateMap<UserUpdateDto, User>()
               .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
               .ForMember(dest => dest.PasswordSalt, opt => opt.Ignore());



        
        }


    }
}
