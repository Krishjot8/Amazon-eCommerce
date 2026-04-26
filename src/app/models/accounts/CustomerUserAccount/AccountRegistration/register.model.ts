export interface CustomerRegister {
  
  firstName: string;
  lastName: string;
  email: string;
  dateOfBirth: Date;
  phoneNumber?: string;
  password: string;
  confirmPassword : string;
  subscribeToNewsLetter: boolean;

}
