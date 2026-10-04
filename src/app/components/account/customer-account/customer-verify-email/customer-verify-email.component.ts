import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { CustomerAuthenticationService } from '../customer-authentication.service';
import { PasswordChallengeVerify } from 'src/app/models/user-authentication/password-challenge/password-challenge-verify.model';
import { UserRole } from 'src/app/models/user-authentication/token/user-role-enum';
import { VerifySms } from 'src/app/models/user-authentication/verification/verify-sms.model';
import { AccountType } from 'src/app/models/user-authentication/verification/account-type-enum';
import { VerifyEmail } from 'src/app/models/user-authentication/verification/verify-email.model';
import { ResendOtpRequest } from 'src/app/models/user-authentication/password-challenge/resend-otp-request.model';

@Component({
  selector: 'app-customer-verify-email',
  templateUrl: './customer-verify-email.component.html',
  styleUrls: ['./customer-verify-email.component.scss']
})
export class CustomerVerifyEmailComponent implements OnInit {

verifyForm: FormGroup;
  email: string = '';
phoneNumber: string = '';
verificationType: 'email' | 'sms' = 'email';
  isSubmitting = false;

  codeResent = false;
  errorMessage: string | null = null;
  resendCooldown = 60; //seconds
  canResend = true;
  resendAttempts = 0;
  interval: any;
serverError: string = '';
  resendSuccessMessage: string | null = ''; // Tracks the green success notification




  constructor(
  private fb: FormBuilder,
  private router: Router,
  private authService: CustomerAuthenticationService
) {

  // Try to get the email from navigation state first
  const nav = this.router.getCurrentNavigation();
  const state = nav?.extras?.state as 
  { email: string;
    phoneNumber?: string;
   };

  if (state?.email) {
this.verificationType = 'email';
    this.email = state.email;
    // Save it to localStorage so it persists on reload
    localStorage.setItem('verificationEmail', this.email);
  } else if(state?.phoneNumber) {
    localStorage.setItem('verificationPhone', this.phoneNumber);
  }else{
this.email = localStorage.getItem('verificationEmail') ||
localStorage.getItem('pendingAuthId') || ''; // Fallback to loginIdentifier if verificationEmail is not set
  this.phoneNumber = localStorage.getItem('verificationPhone') || '';

  if (this.email) {
    this.verificationType = 'email';
  }else if(this.phoneNumber) {
    this.verificationType = 'sms';
  } else {
    // If no email anywhere, redirect to register
    this.router.navigate(['/signin']);
  }
}

  // Initialize the OTP form
  this.verifyForm = this.fb.group({
    otp: ['', [Validators.required, Validators.pattern(/^[0-9]{6}$/)]],
  });
}


  ngOnInit(): void {}

  ngOnDestroy(): void {
    if (this.interval) {
      clearInterval(this.interval);
    }
  }


 
 onSubmit() {
  this.errorMessage = null;

  if (this.verifyForm.invalid) {
    this.verifyForm.markAllAsTouched();
    this.errorMessage = 'Invalid OTP. Please check your code and try again.';
    return;
  }

  this.isSubmitting = true;
  const otpValue = this.verifyForm.value.otp.trim();
  
  if (this.verificationType === 'email') {
    // Get stored pendingAuthId session key, fallback to email if not present
    const authId = localStorage.getItem('pendingAuthId') || this.email;

    const payload: PasswordChallengeVerify = {
      pendingAuthId: authId,
      otp: otpValue,
      role: UserRole.Customer
    };

    console.log('VERIFY PAYLOAD:', payload);

    this.authService.verifyOtp(payload).subscribe({
      next: (response) => this.handleSuccess(response),
      error: (error) => this.handleError(error),
    });
  } else {
    const payload: VerifySms = {
      phoneNumber: this.phoneNumber,
      smsOtpCode: otpValue,
      isResendRequest: false,
      accountType: AccountType.Customer
    };

    this.authService.verifySms(payload).subscribe({
      next: (response) => this.handleSuccess(response),
      error: (error) => this.handleError(error),
    });
  }
}


 onResendOtp() {
  if (!this.canResend) return;

  const authId = localStorage.getItem('pendingAuthId') || 
                 localStorage.getItem('verificationEmail') || 
                 this.email;

  if (!authId) {
    this.errorMessage = 'Session expired. Please start registration again.';
    return;
  }

  const payload: ResendOtpRequest = {
    pendingAuthId: authId,
    role: UserRole.Customer
  };

  console.log('Sending Resend OTP Payload:', payload);

  this.authService.resendEmailOtp(payload).subscribe({
    next: (res) => {
      console.log('Email OTP resent successfully', res);
      this.resendSuccessMessage = 'A new code has been sent to your email.';
      this.errorMessage = null;
      this.resendAttempts++;
      this.codeResent = true;

      this.resendCooldown = res?.cooldownSeconds || (this.resendAttempts === 1 ? 60 : 90);
      this.startCooldown();
    },
    error: (err) => {
      console.error('Failed to resend OTP', err);
      this.resendSuccessMessage = null;

      const errorResponse = err?.error;

      // Check if backend returned remaining cooldown in error payload
      if (errorResponse?.cooldownSeconds) {
        this.resendCooldown = errorResponse.cooldownSeconds;
        this.startCooldown();
      }

      // Display backend error message or fallback
      this.errorMessage = errorResponse?.message || 
                          (typeof errorResponse === 'string' ? errorResponse : 'Session expired or invalid. Please try signing up again.');
    }
  });
}





  startCooldown() {

    this.canResend = false;

    if(this.resendAttempts === 0){
this.resendCooldown = 60;
    }else{
      this.resendCooldown = 90;
    }


    clearInterval(this.interval);

    this.interval = setInterval(() => {
      this.resendCooldown--;

      if (this.resendCooldown <= 0) {
        this.canResend = true;
        clearInterval(this.interval);
      }
    }, 1000);
  }


onInputChange(): void {
  this.errorMessage = null;
  this.codeResent = false;
}

onInputFocus(): void {
  this.errorMessage = null;
  this.codeResent = false;
}
 
  

  private handleSuccess(response: any) {
    console.log('OTP verified successfully', response);
    this.isSubmitting = false;

    const token = response.auth?.token;
    const fullName = response.user?.displayName || '';

if(!token){
  console.error('Token is missing from response', response);
  return;
}

localStorage.clear();

    localStorage.setItem('authToken', token);
    localStorage.setItem('fullName', fullName);


    this.router.navigateByUrl('/').then(() => {
      window.location.reload();
    });
  }


  private handleError(error: any) {
    console.error('OTP verification failed', error);
    this.isSubmitting = false;
   this.errorMessage = 'Invalid OTP. Please check your code and try again.';
  }

}

