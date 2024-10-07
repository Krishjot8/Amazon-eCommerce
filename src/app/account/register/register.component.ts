import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { RegisterService } from './register.service';
import { UserRegistration } from 'src/app/models/register.model';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss'],
})
export class RegisterComponent implements OnInit {
  registrationForm: FormGroup = new FormGroup({});
  submitted = false;

  constructor(
    private router: Router,
    private fb: FormBuilder,
    private registerService: RegisterService
  ) {}

  ngOnInit(): void {
    this.registrationForm = this.fb.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      mobilePhoneNumber: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required, Validators.minLength(6)],
      confirmPassword: ['', Validators.required],
    });
  }

  get f() {
    return this.registrationForm.controls;
  }

  onSubmit(): void {
    this.submitted = true;

    // Stop here if the form is invalid
    if (this.registrationForm.invalid) {
      return;
    }

    // Check if passwords match
    if (
      this.registrationForm.value.password !==
      this.registrationForm.value.confirmPassword
    ) {
      alert('Passwords do not match');
      return;
    }

    const registrationData: UserRegistration = {
      firstName: this.registrationForm.value.firstName,
      lastName: this.registrationForm.value.lastName,
      email: this.registrationForm.value.email,
      mobilePhoneNumber: this.registrationForm.value.mobilePhoneNumber,
      password: this.registrationForm.value.password,
    };

    /*
    this.registerService.registerUser(registrationData).subscribe(
      (response) => {
        console.log('User registered successfully', response);
        this.router.navigate(['/login']);
      },
      (error) => {
        console.error('Error registering user', error);
        alert('Registration failed. Please try again.');
      }
    );
    */
  }
}
