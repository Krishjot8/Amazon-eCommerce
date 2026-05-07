import { UserRole } from "../token/user-role-enum";

export interface ResendOtpRequest {

    pendingAuthId: string;
    role: UserRole
}