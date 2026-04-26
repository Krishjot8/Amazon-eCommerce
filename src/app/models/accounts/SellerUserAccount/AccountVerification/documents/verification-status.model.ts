import { IdentityDocumentType } from "../../AccountRegistration/seller-onboarding/enums/document-type-enum";
import { ProofOfAddressType } from "../../AccountRegistration/seller-onboarding/enums/proof-of-address-enum";

export interface SellerVerificationStatus {

documentType: IdentityDocumentType
documentFrontUrl: string;
documentBackUrl?: string;

registrationExtractUrl: string;
businessAddressProofType: ProofOfAddressType;
businessAddressProofUrl: string;


residentialAddressProofType: ProofOfAddressType;
residentialAddressProofUrl: string;

letterOfAuthorizationUrl: string;



}