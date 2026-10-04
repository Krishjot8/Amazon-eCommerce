import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { CustomerAuthenticationService } from '../customer-authentication.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-customer-reset-password',
  templateUrl: './customer-reset-password.component.html',
  styleUrls: ['./customer-reset-password.component.scss']
})
export class CustomerResetPasswordComponent implements OnInit {

  errorMessage: string = '';
  loginForm!: FormGroup;
  submitted: boolean = false; 

  constructor( private fb: FormBuilder,
    private router: Router,
    private authService: CustomerAuthenticationService,) 
    { }


  ngOnInit(): void {
    this.loginForm = this.fb.group({
      passwordReset: ['',[Validators.required]]
    });
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

   // this.onSubmit();
    }
    

//     onSubmit() {
//       this.submitted = true;
//       this.validateInput();
  
//       const control = this.loginForm.get('emailOrPhone');
//       if (!control || control.invalid) {
//         return;
//       }
  
//       const emailOrPhoneValue = control.value.trim();
  
//       this.authService.checkIdentifier(emailOrPhoneValue).subscribe({
//         next:(response) => {
//    if(response.exists){


// this.authService.storeIdentifier({ passwordReset: emailOrPhoneValue } as any);
// this.router.navigate(['/customer-verification']);
//    }else{

//     this.errorMessage = 'No account found with this Mobile Number or Email Address';
//    }
// },

// error: (err) => {
//   // Handle errors from the API
//   this.errorMessage = err?.error?.message || 'An error occurred. Please try again.';
// }
// })
  
//  } 
}

