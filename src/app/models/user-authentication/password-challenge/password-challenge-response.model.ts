import { OtpChannel } from "./otp-channel-enum";


export interface PasswordChallengeResponse {
  
  pendingAuthId: string;
  otpChannel: OtpChannel;
  maskedDestination: string;
  message: string;
}
