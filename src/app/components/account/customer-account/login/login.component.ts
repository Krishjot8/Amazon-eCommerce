import { Component, OnInit } from '@angular/core';
import {
  AbstractControl,
  FormBuilder,
  FormGroup,
  ValidationErrors,
  Validators,
} from '@angular/forms';
import { AbstractConstructor } from '@angular/material/core/common-behaviors/constructor';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
})
export class CustomerLoginComponent implements OnInit {
  emailOrPhone: string = '';
  errorMessage: string = '';
  loginForm!: FormGroup;

  submitted: boolean = false; //track if input was touched

  constructor(public router: Router, private fb: FormBuilder) {}

  ngOnInit(): void {
    this.loginForm = this.fb.group({
      emailOrPhone: ['', [Validators.required, this.emailOrPhoneValidator]],
    });
  }

  emailOrPhoneValidator(control: AbstractControl): ValidationErrors | null {
    //control: AbstractControl; input control must return null - meaning no error or someError:true;
    const value = control.value?.trim() ?? ''; // control.value gets actual input value; ?.trim()- trims whitespace if the value is not null or undefined

    if (!value) {
      //if input is empty return error of required field
      return { required: true };
    }

    const emailRegex = /^[^@\s]+@[^@\s]+\.[^@\s]+$/;
    const phoneRegex = /^[\d\-]{10,20}$/;

    if (emailRegex.test(value)) return null; //If the input value matches the emailRegex and is valid - return null (no errors)

    if (phoneRegex.test(value)) return null;

    if (value.includes('@')) return { invalidEmail: true }; // if input contains @ but did not pass email regex, it will be an invalid email, therefore return invalidEmail error.

    if (/[a-zA-Z]/.test(value)) return { invalidEmail: true }; // if input contains letters but was not a valid email, then it is not a phone it will be an invalid email, therefore return invalidEmail error.

    return { invalidPhone: true }; // if nothing above is matched its assumed it is a phone number but didnt match, return invalidPhone: true;
  }

  validateInput() {
    const control = this.loginForm.get('emailOrPhone');
    if (!control) return;

    const value = control.value?.trim() ?? '';

    if (control.errors) {
      if (control.errors['required']) {
        this.errorMessage = 'Enter Your Mobile Number or Email Address';
      } else if (control.errors['invalidEmail']) {
        this.errorMessage = 'Invalid Email Address';
      } else if (control.errors['invalidPhone']) {
        this.errorMessage = 'Invalid Mobile Phone Number';
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

    const control = this.loginForm.get('emailOrPhone');
    if (!control || control.invalid) {
      return;
    }

    const emailOrPhoneValue = this.loginForm.get('emailOrPhone')?.value;
    localStorage.setItem('emailOrPhone', emailOrPhoneValue);
    this.router.navigate(['login-password']);
  }
}
