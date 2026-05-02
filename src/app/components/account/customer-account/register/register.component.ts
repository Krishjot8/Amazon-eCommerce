import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { RegisterService } from './register.service';
import { CustomerRegister } from 'src/app/models/accounts/CustomerUserAccount/AccountRegistration/register.model';

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
        fullName: [
          '',
          [
            Validators.required,
            Validators.maxLength(100),
            Validators.pattern(
              /^[A-Za-z' ]+([- ][A-Za-z' ]+)*( (IV|V|VI|VII|VIII|IX|X|XI|XII))?$/
            ),
          ],
        ],
      
       
        
        email: ['', [Validators.required, Validators.email]],
      
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
  
    // ✅ FIRST: validate form
    if (this.registrationForm.invalid) {
      this.registrationForm.markAllAsTouched();
      return;
    }
  
    const fullName: string = this.registrationForm.value.fullName.trim();
  
    const nameParts = fullName
      .split(' ')
      .filter((part: string) => part.length > 0);
  
    // ✅ Full name validation
    if (nameParts.length < 2) {
      this.registrationForm
        .get('fullName')
        ?.setErrors({ invalidFullName: true });
      return;
    }
  
    const firstName = nameParts[0];
    const lastName = nameParts.slice(1).join(' ');
  
    const registrationData: CustomerRegister = {
      firstName,
      lastName,
      email: this.registrationForm.value.email,
      phoneNumber: this.registrationForm.value.phoneNumber,
      password: this.registrationForm.value.password,
      confirmPassword: this.registrationForm.value.confirmPassword,
      subscribeToNewsLetter:
        this.registrationForm.value.subscribeToNewsLetter,
    };
  
    console.log('Form Submitted', registrationData);
  
    this.registerService.registerUser(registrationData).subscribe({
      next: (response) => {
        console.log('Registration successful', response);
  
        this.registrationForm.reset();
        this.submitted = false;
  
        localStorage.setItem('verificationEmail', registrationData.email);
  
        this.router.navigate(['customer-verification']);
      },
      error: (error) => {
        console.error('Registration failed', error);
  
        this.registrationForm.get('email')?.setErrors(null);
  
        if (error.status === 400 && error.error?.message?.includes('email')) {
          this.registrationForm
            .get('email')
            ?.setErrors({ duplicateEmail: true });
        }
      },
    });
  }
}
