import { Component, ElementRef, HostListener, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  Validators,
} from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { CustomerAuthenticationService } from '../customer-authentication.service';
import { Title } from '@angular/platform-browser';
import { parsePhoneNumberWithError, CountryCode as LibCountryCode } from 'libphonenumber-js';
import { countryCodes, CountryCode } from 'src/app/models/user-authentication/country-code/country-code.model';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
})
export class CustomerLoginComponent implements OnInit {
  loginForm!: FormGroup;
  submitted: boolean = false;
  errorMessage: string = '';
  isPasswordReset: boolean = false;

  isDropdownOpen = false;
  
  countryList: CountryCode[] = countryCodes;
  isPhoneInput: boolean = false;
  selectedCountry: CountryCode = this.countryList[0];

  constructor(
    public router: Router,
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private authService: CustomerAuthenticationService,
    private titleService: Title,
    private eRef: ElementRef
  ) {}

  ngOnInit(): void {
    this.loginForm = this.fb.group({
      selectedCountry: ['US'], // Default country
      emailOrPhone: ['', [Validators.required]]
    });

    this.route.queryParams.subscribe(params => {
      if (params['passwordResetSuccess'] === 'true') {
        this.isPasswordReset = true;
      }
    });

    // Detect if input starts with digits or '+' to toggle country dropdown
    this.loginForm.get('emailOrPhone')?.valueChanges.subscribe((value: string) => {
      if (this.submitted) {
        this.submitted = false;
        this.errorMessage = '';
      }

      const trimmed = value?.trim() ?? '';
      this.isPhoneInput = /^\+?\d/.test(trimmed);
    });

    this.titleService.setTitle('Sign-in');
  }



toggleDropdown(event: Event): void {
  event.stopPropagation();
  this.isDropdownOpen = !this.isDropdownOpen;
}

 selectCountry(country: any): void {
  this.loginForm.get('selectedCountry')?.setValue(country.code);
  this.isDropdownOpen = false;
}

getSelectedCountryLabel(): string {
  const currentCode = this.loginForm.get('selectedCountry')?.value;
  const currentCountry = this.countryList.find(c => c.code === currentCode);
  
  // Custom display fallback if no country matches or is selected yet
  return currentCountry ? `${currentCountry.name} ${currentCountry.dialCode}` : 'Select Code';
}

 @HostListener('document:click', ['$event'])
clickout(event: Event) {
  this.isDropdownOpen = false;
}
  

  clearInput(): void {
    this.loginForm.patchValue({ emailOrPhone: '' });
    this.errorMessage = '';
    this.isPhoneInput = false;
  }

  onContinue(): void {
    this.submitted = true;
    const rawValue = this.loginForm.get('emailOrPhone')?.value?.trim() ?? '';

    if (!rawValue) {
      this.errorMessage = 'Enter your mobile number or email address';
      return;
    }


   

    let finalIdentifier = rawValue;

    if (this.isPhoneInput) {
      const selectedCountryCode = this.loginForm.get('selectedCountry')?.value as LibCountryCode;

      try {
        // Strict ITU phone validation using libphonenumber-js
        const phoneNumber = parsePhoneNumberWithError(rawValue, selectedCountryCode);

        if (!phoneNumber.isValid()) {
          this.errorMessage = 'Invalid mobile number';
          return;
        }

        // Format to full international E.164 standard (e.g., +17135550199)
        finalIdentifier = phoneNumber.number;
      } catch (err) {
        this.errorMessage = 'Invalid mobile number';
        return;
      }
    } else {
      // Email validation
      const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
      if (!emailRegex.test(rawValue)) {
        this.errorMessage = 'Invalid email address';
        return;
      }
    }

    this.errorMessage = '';

    // Check backend for existing account
    this.authService.checkIdentifier(finalIdentifier, 0).subscribe({
      next: (res) => {
        if (res && res.exists) {
          localStorage.setItem('loginIdentifier', finalIdentifier);
          this.router.navigate(['/login-password']);
        } else {
          localStorage.setItem('signupIdentifier', finalIdentifier);
          this.router.navigate(['/new-customer-account']);
        }
      },
      error: (err) => {
        console.error('Identifier check failed', err);
      },
    });
  }

   passwordResetSuccess(): boolean {

return this.isPasswordReset;
    }

    
}