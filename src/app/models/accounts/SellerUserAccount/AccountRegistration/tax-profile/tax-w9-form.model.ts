import { BusinessFederalTaxClassification, TaxIdentificationType } from "./enums/tax-enums";
import { LLCFederalTaxCode } from "./enums/w9-enums";
import { Address} from "./tax-profile.model";

export interface SellerTaxW9Form {


    legalName: string;
    businessName: string;
    businessFederalTaxClassification?: BusinessFederalTaxClassification
    llcFederalTaxCode?: LLCFederalTaxCode;

    hasForeignPartnersOrOwners?: boolean;
    exemptPayeeCode?: string;
    fatcaExemptionCode?: string;

    address: Address;

    requesterNameAddress?: string;
    accountNumbers?: string;

    tinType?: TaxIdentificationType;
    tin: string;

    signedBy: string;
    signedDate: Date;
    isElectronicallySigned: boolean;
}