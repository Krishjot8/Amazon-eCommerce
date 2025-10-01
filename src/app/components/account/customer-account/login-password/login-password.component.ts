import { Component, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  Validators,
  AbstractControl,
  ValidationErrors,
} from '@angular/forms';
import { Router } from '@angular/router';
import { CustomerAuthenticationService } from '../customer-authentication.service';
import { CustomerLogin } from 'src/app/models/accounts/customer/customer-login.model';
import { PasswordChallengeResponse } from 'src/app/models/accounts/customer/customer-password-challenge-response';
import { CustomerOtpVerification } from 'src/app/models/accounts/customer/customer-otp-verification.model';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-login-password',
  templateUrl: './login-password.component.html',
  styleUrls: ['./login-password.component.scss'],
})
export class LoginPasswordComponent implements OnInit {
  loginForm!: FormGroup;
  errorMessage: string = ''; // frontend validation
  authErrorMessage: string = ''; // backend auth error
  submitted: boolean = false; // track if user clicked submit

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private authService: CustomerAuthenticationService
  ) {}

  ngOnInit(): void {
    this.loginForm = this.fb.group({
      password: ['', [Validators.required, this.passwordValidator]],
    });

    // clear backend error when typing
    this.loginForm.get('password')?.valueChanges.subscribe(() => {
      this.authErrorMessage = '';
      if (this.submitted) this.validateInput();
    });

    this.loginForm.get('emailOrPhone')?.valueChanges.subscribe(() => {
      if (this.submitted) {
        this.submitted = false; // hide errors as soon as the user types
      }
    });
  }

  passwordValidator(control: AbstractControl): ValidationErrors | null {
    const value = control.value?.trim() ?? '';
    if (!value) return { required: true };

    const passwordRegex = /^(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).+$/;
    if (!passwordRegex.test(value)) return { invalidPassword: true };

    return null;
  }

  validateInput() {
    if (!this.submitted) {
      this.errorMessage = '';
      return;
    }

    const control = this.loginForm.get('password');
    if (!control) return;

    if (control.errors) {
      if (control.errors['required']) {
        this.errorMessage = 'Enter Your Password';
      } else if (control.errors['invalidPassword']) {
        this.errorMessage = 'Password is Incorrect';
      } else {
        this.errorMessage = '';
      }
    } else {
      this.errorMessage = '';
    }
  }


onPasswordInput(){

  this.submitted = false;
  this.authErrorMessage = '';
}

onSubmit() {
  this.submitted = true;

  // Frontend validation first
  if (this.loginForm.invalid) {
    this.authErrorMessage = ''; // no backend error here
    return;
  }

  const password = this.loginForm.get('password')?.value.trim();
  const emailOrPhone = localStorage.getItem('emailOrPhone');

  if (!emailOrPhone) {
    this.router.navigate(['signin']);
    return;
  }

  const loginPayload: CustomerLogin = { emailOrPhone, password };

  this.authService.startPasswordChallenge(loginPayload).subscribe({
    next: (response: PasswordChallengeResponse) => {
      // success: move to OTP verification
      const otpPayload: CustomerOtpVerification = {
        Email: emailOrPhone,
        OTP: '',
        isResendRequest: false,
      };

      localStorage.setItem('pendingAuthId', response.pendingAuthId);
      localStorage.setItem('otpChannel', response.otpChannel);
      localStorage.setItem('otpMasked', response.maskedDestination);
      localStorage.setItem('otpPayload', JSON.stringify(otpPayload));

      this.router.navigate(['customer-verification']);
    },
    error: (err: HttpErrorResponse) => {
      console.log('Login failed', err);

      if (err.status === 401) {
        // Wrong password
        this.authErrorMessage = 'Your password is incorrect';
      } else if (err.status === 404) {
        // No account
        this.authErrorMessage =
          'We cannot find an account with that email or phone';
      } else {
        // Something else
        this.authErrorMessage = 'Something went wrong. Please try again later';
      }
    },
  });
}
}
