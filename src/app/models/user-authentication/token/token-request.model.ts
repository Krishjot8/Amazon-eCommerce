import { UserRole } from "./user-role-enum";

export interface TokenRequest{

    userId: number;
    email : string;
    displayName? : string;
    storeName?: string;
    role: UserRole;
    
    }
    