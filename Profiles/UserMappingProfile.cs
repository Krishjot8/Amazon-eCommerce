using Amazon_eCommerce_API.Models.DBEntities.Users.Business;
using Amazon_eCommerce_API.Models.DBEntities.Users.Customer;
using Amazon_eCommerce_API.Models.DTO_s.Accounts.BusinessUserAccount.AccountUpdate;
using Amazon_eCommerce_API.Models.DTO_s.Accounts.CustomerUserAccount;
using Amazon_eCommerce_API.Models.DTO_s.Accounts.CustomerUserAccount.AccountRegistration;
using Amazon_eCommerce_API.Models.DTO_s.Accounts.CustomerUserAccount.AccountUpdate;
using Amazon_eCommerce_API.Models.DTO_s.Accounts.CustomerUserAccount.Authentication;
using AutoMapper;

namespace Amazon_eCommerce_API.Profiles
{
    public class UserMappingProfile : Profile
    {


        public UserMappingProfile()
        {

            // ======================
            // CUSTOMER - REGISTER
            // ======================

            CreateMap<CustomerUserRegistrationDto, CustomerUser>()
                .ForMember(dest => dest.EmailAddress,
                    opt
                        => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.PasswordHash,
                    opt => opt.Ignore())
                .ForMember(dest => dest.IsEmailVerified,
                    opt => opt.MapFrom(_ => false));
            
            
            // ======================
            // CUSTOMER - UPDATE
            // ======================
            CreateMap<UpdateCustomerUserDto, CustomerUser>()
                .ForMember(dest => dest.PasswordHash, opt
                    => opt.Ignore());
               

            CreateMap<UpdateCustomerUserPasswordDto, CustomerUser>()
                .ForMember(dest => dest.PasswordHash, opt 
                    => opt.Ignore()); // Ignore PasswordHash initially

            
            
            // ======================
            // CUSTOMER - RESPONSE DTO (IMPORTANT)
            // ======================
            
            CreateMap<CustomerUser, CustomerUserDto>()
                .ForMember(dest => dest.Email,
                    opt
                        => opt.MapFrom(src => src.EmailAddress))
                .ForMember(dest => dest.PhoneNumber,
                    opt
                        => opt.MapFrom(src => src.MobileNumber))
                .ForMember(dest => dest.SubscribeToNewsLetter,
                    opt
                        => opt.MapFrom(src => src.CustomerPreferences.SubscribeToNewsletter));
                        
                

//Business


            CreateMap<UpdateBusinessStoreInformationDto, BusinessStoreInformation>()
                .ForMember(dest => dest.BusinessUser, opt => opt.Ignore());

        }


    }
}
