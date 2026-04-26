import { UserRole } from "../token/user-role-enum";

export interface PasswordChallengeRequest {
  
   emailOrPhone: string;
   password: string;
   role: UserRole
  }
  