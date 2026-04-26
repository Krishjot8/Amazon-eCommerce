import { IdentityDocumentType } from "./enums/document-type-enum";

export interface SellerPrimaryContactInformation

{

    firstName: string;
    middleName?: string;
    lastName: string;
    countryOfCitizenship: string;
    countryOfBirth: string;

    birthDay: number;
    birthMonth: number;
    birthYear: number;

    identityDocumentType: IdentityDocumentType
    identityProofNumber: string;
    identityProofExpirationDate: string;

    countryOfIssue: string;

    addressLine1: string;
    addressLine2?: string;

    city: string;
    state: string;
    zipCode: string;
    country: string; //prefilled


    personalPhoneNumber: string;

    pointOfContactIsBeneficialOwner :boolean;
    pointOfContactIsLegalRepresentative :boolean;
    primaryContactIsBeneficialOwner: boolean;
    confirmedActingOnBehalfOfBusiness: boolean;
}