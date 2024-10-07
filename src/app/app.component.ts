import { Component } from '@angular/core';
import { Router } from '@angular/router';
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {

  title = 'Amazon-eCommerce';

  constructor(public router: Router){}

  Signin() {
    return this.router.url === '/signin';
  }

  Register(){
    return this.router.url === '/register';

  }
}
