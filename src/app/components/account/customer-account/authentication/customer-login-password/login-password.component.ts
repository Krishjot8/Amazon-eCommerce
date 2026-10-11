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
import { CustomerLogin } from 'src/app/models/accounts/CustomerUserAccount/Authentication/login.model';
import { PasswordChallengeResponse } from 'src/app/models/user-authentication/password-challenge/password-challenge-response.model';
import { HttpErrorResponse } from '@angular/common/http';
import { Title } from '@angular/platform-browser';

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
  emailOrPhone: string | null = null;

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private authService: CustomerAuthenticationService,
    private titleService: Title
  ) {}

  ngOnInit(): void {
    this.loginForm = this.fb.group({
      password: ['', [Validators.required]],
    });

    // clear backend error when typing
    this.loginForm.get('password')?.valueChanges.subscribe(() => {
      this.authErrorMessage = '';
      if (this.submitted) this.validateInput();
    });

     this.emailOrPhone = localStorage.getItem('loginIdentifier') ?? '',
     this.titleService.setTitle('Amazon Sign-in');
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
  const emailOrPhone = localStorage.getItem('loginIdentifier'); // get email/phone from local storage

  if (!emailOrPhone) {
    this.router.navigate(['/signin']);
    return;
  }

  const loginPayload: CustomerLogin = { emailOrPhone, password };

  this.authService.startPasswordChallenge(loginPayload).subscribe({
    next: (response: PasswordChallengeResponse) => {
      // success: move to OTP verification

      console.log('Generate OTP response:', response); 

     

      if(response?.pendingAuthId){
        localStorage.setItem('pendingAuthId', response.pendingAuthId);

      }else{

        console.error('Missing pendingAuthId', response);
        return
      }
    
if(response?.otpChannel !== null && response?.otpChannel !== undefined){

   localStorage.setItem('otpChannel', response.otpChannel.toString());
}

if(response?.maskedDestination){
      localStorage.setItem('otpMasked', response.maskedDestination);
}



      localStorage.removeItem('loginIdentifier'); // clear email/phone from storage for security
     
      this.router.navigate(['/customer-verification']);
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
