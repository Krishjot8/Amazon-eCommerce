using Amazon_eCommerce_API.Models.DBEntities.Users.Customer;
using Amazon_eCommerce_API.Models.DTO_s.Accounts.CustomerUserAccount.AccountRegistration;
using Amazon_eCommerce_API.Models.DTO_s.Accounts.CustomerUserAccount.Authentication;
using Amazon_eCommerce_API.Models.DTO_s.Accounts.CustomerUserAccount.Password;
using AutoMapper;

namespace Amazon_eCommerce_API.Profiles
{
    public class UserMappingProfile : Profile
    {


        public UserMappingProfile() {


            CreateMap<CustomerUserRegistrationDto, CustomerUser>()
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());


            CreateMap<CustomerUserUpdateDto, CustomerUser>()
               .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());
               

            CreateMap<CustomerUserPasswordUpdateDto, CustomerUser>()
         .ForMember(dest => dest.PasswordHash, opt => opt.Ignore()); // Ignore PasswordHash initially


            CreateMap<CustomerUser, CustomerUserRegistrationDto>()
              .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Username))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.DateofBirth, opt => opt.Ignore()) // Ignoring DateofBirth as it's not in User
                .ForMember(dest => dest.SubscribeToNewsLetter, opt => opt.Ignore()); // Ignore SubscribeToNewsLetter since it's not in User


            

        }


    }
}
