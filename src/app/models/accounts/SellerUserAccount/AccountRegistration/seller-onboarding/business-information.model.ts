import { PinDeliveryMethod } from "./enums/pin-delivery-method-enum";
import { ProofOfAddressType } from "./enums/proof-of-address-enum";

export interface SellerBusinessInformation {


    businessName: string;
    companyRegistrationNumber: string;
    country: string; //prefilled
   addressLine1: string;
   addressLine2?: string;
    city: string;
    state: string;
    zipCode: string;
    proofOfAddress: ProofOfAddressType;
    proofOfAddressDocumentUrl: string;
    registrationExtractUrl?: string;
  pinDeliveryMethod: PinDeliveryMethod;
verificationPhoneNumber: string;
verificationLanguage: string;
   captchaInput: string;
   captchaKey: string;


     


}