import { DocumentStatus } from "./enums/document-status-enum";
import { VerificationCategory } from "./enums/verification-category-enum";


export interface SellerVerificationDocument

{
    id?:string;
    category: VerificationCategory;
    documentType: number;
    documentUrl: string;
    documentBackUrl?: string; // Optional field 
    issuanceDate?: Date;
    expiryDate?: Date;
    status: DocumentStatus;
    rejectionReason?: string; // Optional field for rejection reason



}