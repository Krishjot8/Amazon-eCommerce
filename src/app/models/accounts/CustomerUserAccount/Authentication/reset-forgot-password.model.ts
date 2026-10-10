export interface ResetForgotPassword{


   identifier: string;
   resetToken: string;
    newPassword: string;
    confirmNewPassword: string;
    accountType: number;

}
