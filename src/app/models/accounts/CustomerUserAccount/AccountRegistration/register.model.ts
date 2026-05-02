export interface CustomerRegister {
  
  firstName: string;
  lastName: string;
  email: string;
  phoneNumber?: string;
  password: string;
  confirmPassword : string;
  subscribeToNewsLetter: boolean;

}
