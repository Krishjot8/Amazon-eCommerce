
using Amazon_eCommerce_API.Models.DTO_s.Accounts.SellerUserAccount.SellerRegistration.TaxProfile;

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

            // =============================
            // INDIVIDUAL RULES
            // =============================
            if (dto.TaxClassification == TaxClassification.Individual)
            {
                if (dto.BusinessFederalTaxClassification != 
                    BusinessFederalTaxClassification.IndividualSoleProprietor)
                {
                    errors.Add("Individuals must be classified as Individual / Sole Proprietor.");
                }
                
                if (dto.LLCType != null)
                {
                    errors.Add("Individuals cannot use an LLC Type.");
                }

                if (dto.TaxpayerIdentificationType == 
                    TaxIdentificationType.EIN)
                {
                    errors.Add("Individuals cannot use an EIN.");
                }
            }

            // =============================
            // BUSINESS RULES
            // =============================
            if (dto.TaxClassification == TaxClassification.Business)
            {
                if (dto.BusinessFederalTaxClassification == null)
                {
                    errors.Add("Business federal tax classification is required.");
                }
                
                if (dto.BusinessFederalTaxClassification == 
                    BusinessFederalTaxClassification.LimitedLiabilityCompany
                    && dto.LLCType == null)
                {
                    errors.Add("LLC Type is required when business is LLC.");
                }
                

                if (dto.TaxpayerIdentificationType != 
                    TaxIdentificationType.EIN)
                {
                    errors.Add("Businesses must use an EIN.");
                }

              
            }

            return errors;
        }
    }
}

