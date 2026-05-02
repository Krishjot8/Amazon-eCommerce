import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { CustomerAuthenticationService } from '../customer-authentication.service';
import { VerifyEmail } from 'src/app/models/user-authentication/verification/verify-email.model';
import { AccountType } from 'src/app/models/user-authentication/verification/account-type-enum';
import { VerifySms } from 'src/app/models/user-authentication/verification/verify-sms.model';
import { PasswordChallengeVerify } from 'src/app/models/user-authentication/password-challenge/password-challenge-verify.model';
import { UserRole } from 'src/app/models/user-authentication/token/user-role-enum';

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
verificationType: 'email' | 'sms' = 'email';
  isSubmitting = false;

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


  ngOnInit(): void {}

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

    this.isSubmitting = true;
    const otpValue = this.verifyForm.value.otp.trim();
    
    if(this.verificationType === 'email'){

      const payload: PasswordChallengeVerify = {

        pendingAuthId: this.email,
        otp: this.verifyForm.value.otp,
        role: UserRole.Customer
      };

      console.log('VERIFY PAYLOAD:', payload);

      this.authService.verifyOtp(payload).subscribe({
    next: (response) => this.handleSuccess(response),
      error: (error) => this.handleError(error),

      });

    } else{

      const payload: VerifySms = {

        phoneNumber: this.phoneNumber,
       smsOtpCode: otpValue,
        isResendRequest: false,
        accountType: AccountType.Customer
      };


      this.authService.verifySms(payload).subscribe({
        next: (response) => this.handleSuccess(response),
        error: (error) => this.handleError(error),
      })
    }
  }



  onResendOtp() {



    if(this.verificationType === 'email'){

   
      const payload: VerifyEmail = {

        email: this.email,
        emailOtp: '',
        isResendRequest: true,
        accountType: AccountType.Customer
      };
  
      this.authService.resendEmailOtp(payload).subscribe({
next: (res) =>  console.log('Email OTP resent ', res),
error: (err) => console.error('Failed to resend OTP', err),

    });


  } else{

    const payload: VerifySms = {

      phoneNumber: this.phoneNumber,
     smsOtpCode: '',
      isResendRequest: true,
      accountType: AccountType.Customer
    };

this.authService.resendSmsOtp(payload).subscribe({
  next: (res) => console.log('SMS OTP resent ', res),
  error: (err) => console.error('Failed to resend OTP', err),
    });
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
    // You can also set an error message here to display in the template

  }

}


