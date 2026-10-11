import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { RegisterService } from './register.service';
import { CustomerRegister } from 'src/app/models/accounts/CustomerUserAccount/AccountRegistration/register.model';
import { Title } from '@angular/platform-browser';
import { firstValueFrom } from 'rxjs';
import { CountryCode } from 'src/app/models/user-authentication/country-code/country-code.model';
import { parsePhoneNumberWithError } from 'libphonenumber-js';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss'],
})
export class RegisterComponent implements OnInit, OnDestroy {
  registrationForm: FormGroup = new FormGroup({});
  submitted = false;
  detectedIdentifier: string = '';
  isPhoneRegistration: boolean = false;
  countryCode: CountryCode | undefined;
  countryIsoCode: string = '';
  formattedIdentifier: string = '';

  constructor(
    private router: Router,
    private fb: FormBuilder,
    private registerService: RegisterService,
    private titleService: Title
  ) {}

  ngOnInit(): void {
    // 1. Fetch identifier saved during sign-in
    this.detectedIdentifier = localStorage.getItem('signupIdentifier') ?? '';

    // If no identifier found, send user back to signin
    if (!this.detectedIdentifier) {
      this.router.navigate(['/signin']);
      return;
    }

    // 2. Determine if identifier is phone number or email
    this.isPhoneRegistration = !this.detectedIdentifier.includes('@');

    if (this.isPhoneRegistration) {
      try {
        const parsed = parsePhoneNumberWithError(this.detectedIdentifier);
        if (parsed) {
          // Dynamic ISO code (e.g. "US", "IN", "GB")
          this.countryIsoCode = parsed.country ?? '';

          // Format with space right after dial code: "+1 8322023129"
          const callingCode = '+' + parsed.countryCallingCode;
          const nationalNumber = parsed.nationalNumber;
          this.formattedIdentifier = `${callingCode} ${nationalNumber}`;
        }
      } catch (e) {
        this.countryIsoCode = '';
        this.formattedIdentifier = this.detectedIdentifier;
      }
    } else {
      this.formattedIdentifier = this.detectedIdentifier;
    }

    // 3. Initialize reactive form controls
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

    this.titleService.setTitle('Amazon Registration');
  }

  ngOnDestroy(): void {}

  onChangeIdentifier(): void {
    localStorage.removeItem('signupIdentifier');
    this.router.navigate(['/signin']);
  }

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
      confirmPasswordControl.setErrors({ mismatch: true });
    } else {
      confirmPasswordControl.setErrors(null);
    }

    return null;
  }

  async onSubmit(): Promise<void> {
    this.submitted = true;

    if (this.registrationForm.invalid) {
      this.registrationForm.markAllAsTouched();
      return;
    }

    const fullName: string = this.registrationForm.value.fullName.trim();
    const nameParts = fullName
      .split(' ')
      .filter((part: string) => part.length > 0);

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
      email: this.isPhoneRegistration ? '' : this.detectedIdentifier,
      phoneNumber: this.isPhoneRegistration ? this.detectedIdentifier : '',
      password: this.registrationForm.value.password,
      confirmPassword: this.registrationForm.value.confirmPassword,
      subscribeToNewsLetter:
        this.registrationForm.value.subscribeToNewsLetter,
    };

    try {
      const response = await firstValueFrom(
        this.registerService.registerUser(registrationData)
      );
      console.log('Registration successful', response);

////
if (response?.pendingAuthId) {
      localStorage.setItem('pendingAuthId', response.pendingAuthId);
    }

    localStorage.setItem('verificationEmail', response.email);

    this.router.navigate(['/verify-email'], { state: { email: response.email } });
//////

      this.registrationForm.reset();
      this.submitted = false;

      // Clean up signup state and store the verified identifier (email or phone)
      localStorage.removeItem('signupIdentifier');

      if (this.isPhoneRegistration) {
        localStorage.setItem('verificationPhone', this.detectedIdentifier);
        localStorage.removeItem('verificationEmail');
        this.router.navigate(['/customer-verify-email'], {
          state: { phoneNumber: this.detectedIdentifier },
        });
      } else {
        localStorage.setItem('verificationEmail', this.detectedIdentifier);
        localStorage.removeItem('verificationPhone');
        this.router.navigate(['/customer-verify-email'], {
          state: { email: this.detectedIdentifier },
        });
      }
    } catch (error: any) {
      console.error('Registration failed', error);
    }
  }
}