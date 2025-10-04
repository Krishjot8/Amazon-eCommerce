import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { CustomerAuthenticationService } from '../customer-authentication.service';
import { CustomerOtpVerification } from 'src/app/models/accounts/customer/customer-otp-verification.model';

@Component({
  selector: 'app-customer-verification',
  templateUrl: './customer-verification.component.html',
  styleUrls: ['./customer-verification.component.scss'],
})
export class CustomerVerificationComponent implements OnInit {
  verifyForm: FormGroup;
  email: string = '';
maskedEmail: string = '';
  isSubmitting = false;

  constructor(
  private fb: FormBuilder,
  private router: Router,
  private authService: CustomerAuthenticationService
) {
  // Try to get the email from navigation state first
  const nav = this.router.getCurrentNavigation();
  const state = nav?.extras?.state as { email: string };

  if (state && state.email) {
    this.email = state.email;

    // Save it to localStorage so it persists on reload
    localStorage.setItem('verificationEmail', this.email);
  } else {
    // Try to get it from localStorage
    this.email = localStorage.getItem('verificationEmail') || '';
  }

  if (this.email) {
    this.maskedEmail = this.maskEmail(this.email);
  } else {
    // If no email anywhere, redirect to register
    this.router.navigate(['/register']);
  }

  // Initialize the OTP form
  this.verifyForm = this.fb.group({
    otp: ['', [Validators.required, Validators.pattern(/^[0-9]{6}$/)]],
  });
}

  ngOnInit(): void {}

  private maskEmail(email: string): string {
    const [name, domain] = email.split('@');
    const maskedName = name.substring(0, 2) + '***';
    return `${maskedName}@${domain}`;
  }


  onSubmit() {
    if (this.verifyForm.invalid) {
      this.verifyForm.markAllAsTouched();
      return;
    }

    this.isSubmitting = true;


const payload: CustomerOtpVerification = {

  Email: this.email,
  OTP: this.verifyForm.value.otp,
  isResendRequest: false
};




    this.authService.verifyOtp(payload).subscribe({
      next: (response) => {
        console.log('OTP verified successfully', response);
        this.isSubmitting = false;


localStorage.setItem('authToken', response.token);
localStorage.setItem('firstName', response.firstName);

this.router.navigateByUrl('/').then(() => {

window.location.reload();

 });
},
      error: (error) => {
        console.error('OTP verification failed', error);
        this.isSubmitting = false;
      },
    });
  }

  onResendOtp() {

    const payload: CustomerOtpVerification = {

      Email: this.email,
      OTP: '',
     isResendRequest: true,
    };

    this.authService.resendOtp(payload).subscribe({
next: (res) =>  console.log('OTP resent successfully', res),
error: (err) => console.error('Failed to resend OTP', err),

    })
  }
}
