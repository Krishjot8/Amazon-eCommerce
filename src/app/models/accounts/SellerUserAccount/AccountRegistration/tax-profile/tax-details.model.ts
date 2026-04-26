import { BusinessFederalTaxClassification, LimitationOfBenefits, LLCType, LocationOfServices, TypeOfBeneficialOwner } from "./enums/tax-enums";

export interface IndividualTaxDetails{

fullName: string;
isUSPerson: boolean;
countryOfCitizenship: string;
isRecievingPaymentsOnBehalfOfAnother: boolean;

}


export interface BusinessTaxDetails{

    legalName: string;
    dbaName?: string;
    isUSResidentEntity: boolean;
    businessFederalTaxClassification?: BusinessFederalTaxClassification;
    llcType?: LLCType;
     TypeOfBeneficialOwner?: TypeOfBeneficialOwner;
     locationOfServices: LocationOfServices;
     percentageOfServicesPerformedInUS: number;
     limitationOfBenefits?: LimitationOfBenefits;
     isRecievingPaymentsOnBehalfOfAnother: boolean;


}