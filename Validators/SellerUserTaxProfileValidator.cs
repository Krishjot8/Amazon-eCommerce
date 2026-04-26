
using Amazon_eCommerce_API.Models.DTO_s.Accounts.SellerUserAccount.AccountRegistration.TaxProfile;

namespace Amazon_eCommerce_API.Validators
{
    public class SellerUserTaxProfileValidator
    {
        public List<string> Validate(SellerUserTaxProfileDto dto)
        {
            var errors = new List<string>();

            if (dto == null)
            {
                errors.Add("Tax profile data is required.");
                return errors;
            }
            
            if(dto.TaxIdentification == null)
            {
                errors.Add("TaxIdentification is required.");
            }


            // =============================
            // INDIVIDUAL RULES
            // =============================
            if (dto.TaxClassification == TaxClassification.Individual)
            {
                if (dto.TaxIdentification?.TaxIdentificationType == TaxIdentificationType.EIN)
                {
                    errors.Add("Individuals cannot use an EIN.");
                }
                
                if (dto.Business != null)
                {
                    errors.Add("Individuals cannot provide business details.");
                }

                if (dto.Individual?.IsUSPerson == false && string.IsNullOrEmpty(dto.Individual.CountryOfCitizenship))
                {
                    errors.Add("Non-US individuals must provide country of citizenship");
                }
            }

            // =============================
            // BUSINESS RULES
            // =============================
            if (dto.TaxClassification == TaxClassification.Business)
            {
                if (dto.Business == null)
                {
                    errors.Add("Business details are required.");
                }
                
                if (dto.Business.BusinessFederalTaxClassification == null)
                {
                    errors.Add("Business federal tax classification is required.");
                }

                
                if (dto.Business.BusinessFederalTaxClassification == BusinessFederalTaxClassification.LimitedLiabilityCompany
                    && dto.Business.LLCType == null)
                {
                    errors.Add("LLC Type is required when business is LLC.");
                }
                

                if (dto.TaxIdentification?.TaxIdentificationType != TaxIdentificationType.EIN)
                    
                {
                    errors.Add("Businesses must use an EIN.");
                }

                
            
              
            }

            return errors;
        }
    }
}

