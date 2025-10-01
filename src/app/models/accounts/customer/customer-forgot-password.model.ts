export interface CustomerForgotPassword{


    email:string;
    phoneNumber?:string;
    otp:string;
    newPassword: string;
    reEnterPassword: string;

}
