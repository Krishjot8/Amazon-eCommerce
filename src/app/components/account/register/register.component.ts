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

  constructor( private router: Router, private fb: FormBuilder, private registerService: RegisterService)
  {

    this.registrationForm = this.fb.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      username: ['',[Validators.required]],
      email: ['', [Validators.required, Validators.email]],
      dateOfBirth:['',[Validators.required]],
      phoneNumber: ['', [Validators.pattern(/^\+?[1-9]\d{0,14}$/)]],
      password: ['', [Validators.required, Validators.minLength(6),
        Validators.pattern(/^(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).+$/)
      ]],
      confirmPassword: ['', [Validators.required]],
      subscribeToNewsLetter:[false]
    },
      {
        validators: this.passwordMatchValidator
      });


  }

  ngOnInit(): void {}



private passwordMatchValidator (form: FormGroup){
return form.get('password')?.value === form.get('confirmPassword')?.value
?null: {mismatch:true};

}




  onSubmit(): void {

if(this.registrationForm.valid){

  const registrationData: UserRegistration = {
    firstName: this.registrationForm.value.firstName,
    lastName: this.registrationForm.value.lastName,
    username:  this.registrationForm.value.username,
    email:  this.registrationForm.value.email,
    dateOfBirth: this.registrationForm.value.dateOfBirth ,
    phoneNumber: this.registrationForm.value.phoneNumber,
    password: this.registrationForm.value.password,
    confirmPassword:this.registrationForm.value.confirmPassword,
    subscribeToNewsLetter: false
  };

  this.registerService.registerUser(registrationData).subscribe({
    next: (response) => {
      console.log('Registration successful', response);
      // Handle success (e.g., redirect to login)
    },
    error: (error) => {
      console.error('Registration failed', error);
      // Handle error (e.g., show error message)
    }
  });

}

  }
}
