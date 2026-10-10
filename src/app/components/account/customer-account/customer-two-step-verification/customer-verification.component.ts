import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { CustomerAuthenticationService } from '../customer-authentication.service';

import { AccountType } from 'src/app/models/user-authentication/verification/account-type-enum';
import { VerifySms } from 'src/app/models/user-authentication/verification/verify-sms.model';
import { PasswordChallengeVerify } from 'src/app/models/user-authentication/password-challenge/password-challenge-verify.model';
import { UserRole } from 'src/app/models/user-authentication/token/user-role-enum';
import { ResendOtpRequest } from 'src/app/models/user-authentication/password-challenge/resend-otp-request.model';

@Component({
  selector: 'app-customer-verification',
  templateUrl: './customer-verification.component.html',
  styleUrls: ['./customer-verification.component.scss'],
})
export class CustomerVerificationComponent implements OnInit {
  verifyForm: FormGroup;
  email: string = '';
maskedEmail: string = '';
phoneNumber: string = '';
serverError: string = '';
verificationType: 'email' | 'sms' = 'email';
  isSubmitting = false;

  // resend code

  resendCooldown = 60; //seconds
  canResend = false;
  resendAttempts = 0;
  interval: any;



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


if(this.email){

this.maskedEmail = this.maskEmail(this.email);

}


  // Initialize the OTP form
  this.verifyForm = this.fb.group({
    otp: ['', [Validators.required, Validators.pattern(/^[0-9]{6}$/)]],
  });
}


  ngOnInit(): void {

    this.startCooldown();
  }

  private maskEmail(email: string): string {
    const [name, domain] = email.split('@');
    const maskedName = name.substring(0, 2) + '******';
    return `${maskedName}@${domain}`;
  }

  // verify OTP

  onSubmit() {
  if (this.verifyForm.invalid) {
    this.verifyForm.markAllAsTouched();
    return;
  }

  this.serverError = '';
  this.isSubmitting = true;

  const otpValue = this.verifyForm.value.otp.trim();
  const otpPurpose = localStorage.getItem('otpPurpose');
  const pendingAuthId = localStorage.getItem('pendingAuthId') || this.email;

  // --- 1. PASSWORD RESET FLOW ---

  
  if (otpPurpose === 'PasswordReset') {
    this.authService.verifyPasswordResetOtp(pendingAuthId, otpValue, AccountType.Customer).subscribe({
      next: (response) => {
        this.isSubmitting = false;

        if (response?.success && response?.resetToken) {
          // Store resetToken for the 'Create New Password' page
          localStorage.setItem('resetToken', response.resetToken);

          // Clean up temporary OTP storage keys
          localStorage.removeItem('pendingAuthId');
          localStorage.removeItem('otpPurpose');

          // Navigate to the reset password component
          this.router.navigate(['/customer-reset-password']);
        } else {
          this.serverError = 'Verification failed. Please try again.';
        }
      },
      error: (error) => {
        this.isSubmitting = false;
        this.handleError(error);
      }
    });

    return; // Exit early so standard login flow isn't executed
  }

  // --- 2. STANDARD LOGIN FLOW ---
  if (this.verificationType === 'email') {
    const payload: PasswordChallengeVerify = {
      pendingAuthId: this.email,
      otp: otpValue,
      role: UserRole.Customer
    };

    console.log('VERIFY PAYLOAD:', payload);

    this.authService.verifyOtp(payload).subscribe({
      next: (response) => this.handleSuccess(response),
      error: (error) => this.handleError(error)
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
      error: (error) => this.handleError(error)
    });
  }
}



  onResendOtp() {
  if (!this.canResend) return;

  const otpPurpose = localStorage.getItem('otpPurpose');
  const pendingAuthId = localStorage.getItem('pendingAuthId') || '';

  // --- PASSWORD RESET RESEND ---
  if (otpPurpose === 'PasswordReset') {
    this.authService.generatePasswordResetOtp(pendingAuthId, 0).subscribe({
      next: (res) => {
        console.log('Password reset OTP resent', res);
        this.resendAttempts++;
        this.resendCooldown = res.cooldownSeconds || 30;
        this.startCooldown();
      },
      error: (err) => console.error('Failed to resend password reset OTP', err)
    });
    return;
  }

  // --- STANDARD LOGIN / REGISTRATION RESEND ---
  const payload: ResendOtpRequest = {
    pendingAuthId: pendingAuthId,
    role: 0 
  };

  this.authService.resendEmailOtp(payload).subscribe({
    next: (res) => {
      console.log('Email OTP resent ', res);
      this.resendAttempts++;
      this.resendCooldown = res.cooldownSeconds;
      this.startCooldown();
    },
    error: (err) => console.error('Failed to resend OTP', err)
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

  onInputChange() {
    if (this.serverError) {
      this.serverError = '';
    }
  }

  onInputFocus() {
    if (this.serverError) {
      this.serverError = '';
    }
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

    if(typeof error.error === 'string'){
      this.serverError = error.error;
    }else if(error.error?.message){
      this.serverError = error.error.message;
    }else{
      this.serverError = 'The code you entered is invalid or has expired. Please check the code and try again.';
    }
    // You can also set an error message here to display in the template

  }

}


