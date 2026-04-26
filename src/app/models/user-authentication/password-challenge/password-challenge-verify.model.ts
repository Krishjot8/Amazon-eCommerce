import { UserRole } from "../token/user-role-enum";


export interface PasswordChallengeVerify {
  
  pendingAuthId: string;
  otp: string;
  role: UserRole
}
