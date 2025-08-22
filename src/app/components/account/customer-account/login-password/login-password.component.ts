import { Component, OnInit } from '@angular/core';
import {
  AbstractControl,
  FormBuilder,
  FormGroup,
  ValidationErrors,
  Validators,
} from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login-password',
  templateUrl: './login-password.component.html',
  styleUrls: ['./login-password.component.scss'],
})
export class LoginPasswordComponent implements OnInit {
  loginForm!: FormGroup;
  errorMessage: string = '';
  authErrorMessage: string = '';
  submitted: boolean = false; // Track if the user pressed Submit

  constructor(private fb: FormBuilder, private router: Router) {}

  ngOnInit(): void {
    this.loginForm = this.fb.group({
      password: ['', [Validators.required, this.passwordValidator]],
    });

    this.loginForm.get('password')?.valueChanges.subscribe(() => {
      if (this.submitted) this.validateInput();
      this.authErrorMessage = '';
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
      this.errorMessage = ''; // Only show error after Submit
      return;
    }

    const control = this.loginForm.get('password');
    if (!control) return;

    if (control.errors) {
      if (control.errors['required']) {
        this.errorMessage = 'Enter Your Password';
      } else if (control.errors['invalidPassword']) {
        this.errorMessage = 'Your Password is Incorrect';
      } else {
        this.errorMessage = '';
      }
    } else {
      this.errorMessage = '';
    }
  }

  onSubmit() {
    this.submitted = true;
    this.validateInput();

    const control = this.loginForm.get('password');
    if (!control || control.invalid) return;

    const passwordValue = control.value;

    localStorage.setItem('password', passwordValue);
    this.router.navigate(['']); // Navigate to next page
  }
}
