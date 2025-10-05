import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { RegisterService } from './register.service';
import { CustomerRegistration } from 'src/app/models/accounts/customer/customer-register.model';

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
    this.registrationForm = this.fb.group(
      {
        firstName: [
          '',
          [
            Validators.required,
            Validators.maxLength(45),
            Validators.pattern(
              /^[A-Za-z' ]+([- ][A-Za-z' ]+)*( (IV|V|VI|VII|VIII|IX|X|XI|XII))?$/
            ),
          ],
        ],
        lastName: [
          '',
          [
            Validators.required,
            Validators.maxLength(45),
            Validators.pattern(
              /^[A-Za-z' ]+([- ][A-Za-z' ]+)*( (IV|V|VI|VII|VIII|IX|X|XI|XII))?$/
            ),
          ],
        ],
        username: [
          '',
          [
            Validators.required,
            Validators.minLength(5),
            Validators.maxLength(20),
          ],
        ],
        email: ['', [Validators.required, Validators.email]],
        dateOfBirth: ['', [Validators.required]],
        phoneNumber: [
          '',
          [
            Validators.pattern(
              /^(\+?\d{1,3}[-. ]?)?(\(?\d{1,4}\)?[-. ]?)?(\d{1,4}[-. ]?)?(\d{1,4}[-. ]?)?(\d{1,9})$/
            ),
          ],
        ],
        password: [
          '',
          [
            Validators.required,
            Validators.minLength(6),
            Validators.pattern(/^(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).+$/),
          ],
        ],
        confirmPassword: ['', [Validators.required]],
        subscribeToNewsLetter: [false],
      },
      { validators: this.passwordMatchValidator }
    );
  }

  ngOnDestroy(): void {}

  private passwordMatchValidator(formGroup: FormGroup) {
    const passwordControl = formGroup.get('password');
    const confirmPasswordControl = formGroup.get('confirmPassword');

    if (!passwordControl || !confirmPasswordControl) {
      return null;
    }

    if (
      confirmPasswordControl.errors &&
      !confirmPasswordControl.errors['mismatch']
    ) {
      return null;
    }

    if (passwordControl.value !== confirmPasswordControl.value) {
      confirmPasswordControl.setErrors({ mismatch: true }); // Set the error on confirmPassword
    } else {
      confirmPasswordControl.setErrors(null); // Clear the error if passwords match
    }

    return null;
  }

  onSubmit(): void {
    this.submitted = true;

    if (this.registrationForm.valid) {
      const registrationData: CustomerRegistration = {
        firstName: this.registrationForm.value.firstName,
        lastName: this.registrationForm.value.lastName,
        username: this.registrationForm.value.username,
        email: this.registrationForm.value.email,
        dateOfBirth: this.registrationForm.value.dateOfBirth,
        phoneNumber: this.registrationForm.value.phoneNumber,
        password: this.registrationForm.value.password,
        confirmPassword: this.registrationForm.value.confirmPassword,
        subscribeToNewsLetter: this.registrationForm.value.subscribeToNewsLetter,
      };

      console.log(
        'Form Submitted',
        this.registrationForm.value,

      );





      this.registerService.registerUser(registrationData).subscribe({
        next: (response) => {
          console.log('Registration successful', response);
          // Handle success (e.g., redirect to login)
          this.registrationForm.reset();
          this.submitted = false;

 localStorage.setItem('verificationEmail', registrationData.email);

          this.router.navigate(['customer-verification']);
        },
        error: (error) => {
          console.error('Registration failed', error);
          // Handle error (e.g., show error message)

          this.registrationForm.get('email')?.setErrors(null);

          if(error.status === 400 && error.error?.message?.includes('email')){


            this.registrationForm.get('email')?.setErrors({duplicateEmail:true});
          }

        },
      });
    } else {
      this.registrationForm.markAllAsTouched();
    }
  }
}
