import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { CustomerAuthenticationService } from '../customer-authentication.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Title } from '@angular/platform-browser';
import { CustomerLogin } from 'src/app/models/accounts/CustomerUserAccount/Authentication/login.model';
import { PasswordChallengeResponse } from 'src/app/models/user-authentication/password-challenge/password-challenge-response.model';

@Component({
  selector: 'app-customer-forgot-password',
  templateUrl: './customer-forgot-password.component.html',
  styleUrls: ['./customer-forgot-password.component.scss']
})
export class CustomerForgotPasswordComponent implements OnInit {
authErrorMessage: string = '';
  errorMessage: string = '';
  PasswordAssistanceForm!: FormGroup;
  submitted: boolean = false; 

  constructor( private fb: FormBuilder,
    private router: Router,
    private titleService: Title,
    private authService: CustomerAuthenticationService,) 
    { }



  ngOnInit(): void {
    this.PasswordAssistanceForm = this.fb.group({
      emailOrPhone: ['',[Validators.required]]
    });
    this.titleService.setTitle('Amazon Password Assistance');
  }

  validateInput() {
    const control = this.PasswordAssistanceForm.get('emailOrPhone');
    if (!control) return;

    if(control.errors && control.errors['required']) {
      this.errorMessage = 'Enter your email or mobile phone number';
    } else{

      this.errorMessage = '';
    }
  }


  onInputChange() {

    const control = this.PasswordAssistanceForm.get('emailOrPhone');
    if(control && control.value.trim().length > 0) {
      this.errorMessage = '';
    }
  }

  onInputFocus() {
this.errorMessage = '';
this.submitted = false;

  }

  onContinue(){

    this.onSubmit();
    }

  

    onSubmit() {
      this.submitted = true;
      this.validateInput();
  
      const control = this.PasswordAssistanceForm.get('emailOrPhone');
      if (!control || control.invalid) {
        return;
      }
  
      this.authErrorMessage = '';
      const emailOrPhoneValue = control.value.trim();
  
      this.authService.checkIdentifier(emailOrPhoneValue).subscribe({
        next:(checkResponse) => {
   if(checkResponse.exists){


 this.authService.generatePasswordResetOtp(emailOrPhoneValue, 0).subscribe({
  next: (response: PasswordChallengeResponse) => {

  localStorage.setItem('pendingAuthId', emailOrPhoneValue);
      localStorage.setItem('otpPurpose', 'PasswordReset');

  this.router.navigate(['/customer-verification']);
     },
    
  
  error: (challengeErr) => {
    this.authErrorMessage = challengeErr?.error?.message || 'An error occurred while sending the OTP. Please try again.';
   }
  });
  }else{
    this.authErrorMessage = 'I\'m sorry, we couldn\'t identify your account.';
  }
},   
 })
   }
   };
  
  
  

