import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { CustomerAuthenticationService } from '../customer-authentication.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Title } from '@angular/platform-browser';

@Component({
  selector: 'app-customer-forgot-password',
  templateUrl: './customer-forgot-password.component.html',
  styleUrls: ['./customer-forgot-password.component.scss']
})
export class CustomerForgotPasswordComponent implements OnInit {

  errorMessage: string = '';
  loginForm!: FormGroup;
  submitted: boolean = false; 

  constructor( private fb: FormBuilder,
    private router: Router,
    private titleService: Title,
    private authService: CustomerAuthenticationService,) 
    { }



  ngOnInit(): void {
    this.loginForm = this.fb.group({
      emailOrPhone: ['',[Validators.required]]
    });
    this.titleService.setTitle('Amazon Password Assistance');
  }

  validateInput() {
    const control = this.loginForm.get('emailOrPhone');
    if (!control) return;

    if(control.errors && control.errors['required']) {
      this.errorMessage = 'Enter your Mobile Number or Email Address';
    } else{

      this.errorMessage = '';
    }
  }


  onInputChange() {

    const control = this.loginForm.get('emailOrPhone');
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
  
      const control = this.loginForm.get('emailOrPhone');
      if (!control || control.invalid) {
        return;
      }
  
      const emailOrPhoneValue = control.value.trim();
  
      this.authService.checkIdentifier(emailOrPhoneValue).subscribe({
        next:(response) => {
   if(response.exists){


this.authService.storeIdentifier({ emailOrPhone: emailOrPhoneValue } as any);
this.router.navigate(['/customer-verification']);
   }else{

    this.errorMessage = 'No account found with this Mobile Number or Email Address';
   }
},

error: (err) => {
  // Handle errors from the API
  this.errorMessage = err?.error?.message || 'An error occurred. Please try again.';
}
})
  
 } 
}

