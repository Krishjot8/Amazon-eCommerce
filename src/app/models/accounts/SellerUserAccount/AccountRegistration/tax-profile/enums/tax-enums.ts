export enum TaxClassification{

Individual = 0,
Business = 1,

}

export enum TaxIdentificationType
{

    SSN = 0,
    EIN = 1,
    ITIN = 2,
    Other = 3
}


export enum BusinessFederalTaxClassification {

IndividualSoleProprietor = 0,
C_Corporation = 1,
S_Corporation = 2,
Partnership = 3,
TrustOrEstate = 4,
LimitedLiabilityCompany = 5,
Other = 6

}

export enum LLCType{


    S_Corporation = 0,
    C_Corporation = 1,
    Partnership = 2,
}

export enum TypeOfBeneficialOwner
    {
        Corporation = 0,
        Estate = 1,
        ForeignGovernmentControlledEntity = 2,
        ForeignGovernmentIntegralPart = 3,
        InternationalOrganization = 4,
        CentralBankOfIssue = 5,
        TaxExemptOrganization = 6,
        PrivateFoundation = 7,
        ComplexTrust = 8,
        SimpleTrust = 9,
        GrantorTrust = 10,
        Partnership = 11,
        DisregardedEntity = 12
    }

export enum LocationOfServices
{
    OutsideUS,
    InsideUS,
    InsideOutsideUS,
}


export enum LimitationOfBenefits
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
