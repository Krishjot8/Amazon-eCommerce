import { AccountType } from "./account-type-enum";

export interface VerifySms{

    phoneNumber: string;
    smsOtpCode : string;
    isResendRequest: boolean;
    accountType: AccountType;
    
    }
    
   