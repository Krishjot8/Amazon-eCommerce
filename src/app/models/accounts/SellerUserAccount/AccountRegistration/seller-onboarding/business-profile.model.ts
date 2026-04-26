import { BusinessType } from "./enums/business-type-enum";

export interface SellerBusinessProfile {

    BusinessName: string;
    BusinessType: BusinessType;
    Country: string;
    AgreeToTerms: boolean;
    
    }