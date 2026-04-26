export interface SellerForgotPassword{


    email:string;
    phoneNumber?:string;
    otp:string;
    newPassword: string;
    confirmNewPassword: string;

}
