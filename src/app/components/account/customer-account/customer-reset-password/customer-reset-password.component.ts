import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { CustomerAuthenticationService } from '../customer-authentication.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ResetForgotPassword } from 'src/app/models/accounts/CustomerUserAccount/Authentication/reset-forgot-password.model';

@Component({
  selector: 'app-customer-reset-password',
  templateUrl: './customer-reset-password.component.html',
  styleUrls: ['./customer-reset-password.component.scss']
})
export class CustomerResetPasswordComponent implements OnInit {

  authErrorMessage: string = '';
  errorMessage: string = '';
  passwordResetForm!: FormGroup;
  submitted: boolean = false; 


  constructor( private fb: FormBuilder,
    private router: Router,
    private authService: CustomerAuthenticationService,) 
    { }


 ngOnInit(): void {
  this.passwordResetForm = this.fb.group({
    newPassword: [
      '',
      [
        Validators.required,
        Validators.minLength(6),
        Validators.pattern(/^(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).+$/),
      ]
    ],
    confirmNewPassword: ['', [Validators.required]]
  }, { validators: this.passwordMatchValidator });
}

  validateInput() {
    const control = this.passwordResetForm.get('newPassword');
    if (!control) return;

    if(control.errors && control.errors['required']) {
      this.errorMessage = 'Enter your new password';
    } else{

      this.errorMessage = '';
    }
  }


  onInputChange() {

    const control = this.passwordResetForm.get('newPassword');
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

      if(this.passwordResetForm.invalid) {
        this.passwordResetForm.markAllAsTouched();
        return;
      }

const token = localStorage.getItem('resetToken') || '';
const identifier = localStorage.getItem('loginIdentifier') || '';


const payload: ResetForgotPassword = {
identifier: identifier,
resetToken: token,
newPassword: this.passwordResetForm.value.newPassword,
confirmNewPassword: this.passwordResetForm.value.confirmNewPassword,
accountType: 0 
};

      this.authService.resetPassword(payload).subscribe({
        next:(response) => {


        this.submitted = false;


          localStorage.removeItem('resetToken');
          localStorage.removeItem('loginIdentifier');
      localStorage.removeItem('pendingAuthId');
      localStorage.removeItem('otpPurpose');
          

          this.router.navigate(['/signin'], {
        queryParams: { passwordResetSuccess: 'true' }
      });
        },
        error: (err) => {
          this.authErrorMessage = err?.error?.message || 'An error occurred during password reset. Please try again.';
        }
      });
    } 


    

  private passwordMatchValidator(formGroup: FormGroup) {
  const newPasswordControl = formGroup.get('newPassword');
  const confirmNewPasswordControl = formGroup.get('confirmNewPassword');

  if (!newPasswordControl || !confirmNewPasswordControl) {
    return null;
  }

  if (newPasswordControl.value !== confirmNewPasswordControl.value) {
    confirmNewPasswordControl.setErrors({ mismatch: true });
  } else {
    // If they match, clear the mismatch error
    if (confirmNewPasswordControl.hasError('mismatch')) {
      confirmNewPasswordControl.setErrors(null);
    }
  }

  // Ensure every code path returns a value (null when no group-level error)
  return null;
}

}

