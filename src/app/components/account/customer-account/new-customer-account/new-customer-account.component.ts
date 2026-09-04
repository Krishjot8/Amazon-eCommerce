import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-new-customer-account',
  templateUrl: './new-customer-account.component.html',
  styleUrls: ['./new-customer-account.component.scss']
})
export class NewCustomerAccountComponent implements OnInit {


  emailOrPhone: string = ''

  constructor(private router: Router) { }

  ngOnInit() : void{

    this.emailOrPhone = localStorage.getItem('signupIdentifier') ?? '';

    if(!this.emailOrPhone) {
      this.router.navigate(['/signin']);
    }
  }

  goToRegister():void{

    this.router.navigate(['/register']);
  }

}
