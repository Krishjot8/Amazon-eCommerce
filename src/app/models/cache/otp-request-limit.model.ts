export interface OtpRequestLimit {
  email: string;
  lastRequestTime: Date;
  expirationMinutes: number;
}
