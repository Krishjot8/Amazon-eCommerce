import { Component, OnInit } from '@angular/core';
import { NavigationEnd, Router, Event } from '@angular/router';
import { filter } from 'rxjs';
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent {
  title = 'Amazon-eCommerce';

showHeaderFooter = true;

  constructor(public router: Router){

   this.router.events
      .pipe(filter((event): event is NavigationEnd => event instanceof NavigationEnd))
      .subscribe((event) => {
        const noHeaderFooterRoutes = ['/signin', '/register'];
        const currentUrl = event.urlAfterRedirects.split('?')[0]; // ignore query params

        this.showHeaderFooter = !noHeaderFooterRoutes.includes(currentUrl);
      });

};



  }




