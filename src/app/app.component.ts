import { Component, OnInit } from '@angular/core';
import { NavigationEnd, Router, Event } from '@angular/router';
import { filter } from 'rxjs';
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent implements OnInit {
  title = 'Amazon-eCommerce';

showHeaderFooter = true;

  constructor(public router: Router){}


  ngOnInit() {
 /*   this.router.events.pipe(
  filter((event: Event): event is NavigationEnd => event instanceof NavigationEnd)
).subscribe(event => {
  const path = event.urlAfterRedirects.toLowerCase().replace(/\/$/, ''); // lowercase & remove trailing slash
 this.showHeaderFooter = !['/signin', '/register'].includes(path);

}); */
}
}
