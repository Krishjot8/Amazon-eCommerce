using System.ComponentModel.DataAnnotations;

namespace Amazon_eCommerce_API.Models.DTO_s.Accounts.SellerUserAccount.SellerRegistration.TaxProfile
{
    public class BusinessTaxDetailsDto
    {
        [Required]
        [StringLength(200)]
        public string LegalName { get; set; }
      
        [StringLength(200,MinimumLength = 2)]
        public string? DBAName { get; set; } 
        
        public bool IsUSResidentEntity { get; set; } 
        
        public BusinessFederalTaxClassification? BusinessFederalTaxClassification {get; set;} //If Business U.S Resident
       
        public LLCType? LLCType { get; set; }
        
     
        public TypeOfBeneficialOwner? TypeOfBeneficialOwner {get; set;} //If Business Non-US Resident

               
        public bool IsHybridEntity { get; set; }//If Business Non-US Resident

         
        public LocationOfServices? LocationOfServices {get; set;} //If Business Non-US Resident

        [Range(0,100)]
        
        public decimal? PercentageOfServicesPerformedInUS { get; set; }
        
        public LimitationOfBenefits? LimitationOfBenefits { get; set; } //If Country qualifies for treaty benefits
        
        public bool IsReceivingPaymentsOnBehalfOfAnother {get; set;} //If Business Non-US Resident Or Individual Classification

    }
    public enum BusinessFederalTaxClassification
    {
        [Display(Name = "Individual / Sole Proprietor")]
        IndividualSoleProprietor,
        [Display(Name = "C Corporation")]
        C_Corporation,
        [Display(Name = "S Corporation")]
        S_Corporation,
        Partnership,
        [Display(Name = "Trust / Estate")]
        TrustOrEstate,
        [Display(Name = "Limited Liability Company")]
        LimitedLiabilityCompany,
        Other
        
    }
    public enum LLCType
    {
        
        [Display(Name = "S Corporation")]
        S_Corporation,
        
        [Display(Name = "C Corporation")]
        C_Corporation,
       
        Partnership
    }

    public enum TypeOfBeneficialOwner
    {
        Corporation,
        Estate,
        [Display(Name = "Foreign Government - Controlled Entity")]
        ForeignGovernmentControlledEntity,
        [Display(Name = "Foreign Government - Integral Part")]
        ForeignGovernmentIntegralPart,
        InternationalOrganization,
        CentralBankOfIssue,
        TaxExemptOrganization,
        PrivateFoundation,
        ComplexTrust,
        SimpleTrust,
        GrantorTrust,
        Partnership,
        DisregardedEntity
    }
    
    public enum LocationOfServices
    {
        OutsideUS,
        InsideUS,
        InsideOutsideUS,
    }


    public enum LimitationOfBenefits
    {
        
        PubliclyTradedCorporation,
        SubsidiaryOfPubliclyTradedCorporation,
        TaxExemptPensionTrustOrFund,
        OtherTaxExemptOrganization,
        CompanyMeetOwnershipBaseErosionTest,
        CompanyMeetDerivativeBenefitsTest,
        CompanyWithItemOfIncomeMeetingActiveTradeOrBusinessTest,
        FavorableDiscretionaryDeterminationByUsCompetentAuthorityReceived
        
    }
}