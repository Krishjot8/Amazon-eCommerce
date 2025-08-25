
export interface PasswordChallengeResponse{

  pendingAuthId: string;
  otpChannel: 'sms' | 'email';
  maskedDestination: string;
}
