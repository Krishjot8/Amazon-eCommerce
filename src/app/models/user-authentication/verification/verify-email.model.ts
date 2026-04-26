import { AccountType } from "./account-type-enum";

export interface VerifyEmail{

email : string;
emailOtp : string;
isResendRequest: boolean;
accountType: AccountType;

}

