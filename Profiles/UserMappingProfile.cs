using Amazon_eCommerce_API.Models.DTO_s;
using Amazon_eCommerce_API.Models.Users;
using AutoMapper;

namespace Amazon_eCommerce_API.Profiles
{
    public class UserMappingProfile : Profile
    {


        public UserMappingProfile() {


            CreateMap<UserRegistrationDto, User>()
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());


            CreateMap<UserUpdateDto, User>()
               .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.Role, opt => opt.Ignore());

            CreateMap<User, UserDto>()
              .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.DateofBirth, opt => opt.Ignore()) // Ignoring DateofBirth as it's not in User
                .ForMember(dest => dest.PhoneNumber, opt => opt.Ignore()) // Ignoring PhoneNumber as it's not in User
                .ForMember(dest => dest.SubscribeToNewsLetter, opt => opt.Ignore()); // Ignore SubscribeToNewsLetter since it's not in User

        }


    }
}
