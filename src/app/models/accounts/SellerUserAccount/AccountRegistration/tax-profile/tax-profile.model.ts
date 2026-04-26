import { TaxClassification, TaxIdentificationType } from "./enums/tax-enums";
import { BusinessTaxDetails, IndividualTaxDetails } from "./tax-details.model";

export interface SellerUserTaxProfile {

taxClassification: TaxClassification
taxCountryCode: string;
taxIdentification: TaxIdentificationType;
permanentAddress: Address;
mailingAddress?: Address;
individual?: IndividualTaxDetails;
business?: BusinessTaxDetails;
}

export interface TaxIdentification{


    countryCode: string;
    taxIdentificationType: TaxIdentificationType;
    taxIdentificationNumber: string;
}

export interface Address {
country: string;
addressLine1: string;
addressLine2?: string;
city: string;
state: string;
zipCode: string;

}