using Amazon_eCommerce_API.Models.DBEntities.Users.Business;
using Amazon_eCommerce_API.Models.DBEntities.Users.Customer;
using Amazon_eCommerce_API.Models.DTO_s.Accounts.BusinessUserAccount.AccountUpdate;
using Amazon_eCommerce_API.Models.DTO_s.Accounts.CustomerUserAccount.AccountRegistration;
using Amazon_eCommerce_API.Models.DTO_s.Accounts.CustomerUserAccount.AccountUpdate;
using Amazon_eCommerce_API.Models.DTO_s.Accounts.CustomerUserAccount.Authentication;
using AutoMapper;

namespace Amazon_eCommerce_API.Profiles
{
    public class UserMappingProfile : Profile
    {


        public UserMappingProfile() {


            CreateMap<CustomerUserRegistrationDto, CustomerUser>()
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());


            CreateMap<UpdateCustomerUserDto, CustomerUser>()
               .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());
               

            CreateMap<UpdateCustomerUserPasswordDto, CustomerUser>()
         .ForMember(dest => dest.PasswordHash, opt => opt.Ignore()); // Ignore PasswordHash initially


            CreateMap<CustomerUser, CustomerUserRegistrationDto>()
              .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.EmailAddress))
                .ForMember(dest => dest.DateofBirth, opt => opt.Ignore()) // Ignoring DateofBirth as it's not in User
                .ForMember(dest => dest.SubscribeToNewsLetter, opt => opt.Ignore()); // Ignore SubscribeToNewsLetter since it's not in User


//Business


            CreateMap<UpdateBusinessStoreInformationDto, BusinessStoreInformation>()
                .ForMember(dest => dest.BusinessUser.Id, opt => opt.Ignore())
                .ForMember(dest => dest.BusinessUser, opt => opt.Ignore());

        }


    }
}
