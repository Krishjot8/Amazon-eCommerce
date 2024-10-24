export interface UserRegistration {
  firstName: string;
  lastName: string;
  username: string;
  email: string;
  dateOfBirth: Date;
  phoneNumber?: string;
  password: string;
  confirmPassword : string;
  subscribeToNewsLetter: boolean;

}
