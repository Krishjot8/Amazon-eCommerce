import { Component } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';
import { filter } from 'rxjs';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent {
  title = 'Amazon-eCommerce';
  showHeaderFooter = true;

  constructor(public router: Router) {
    this.router.events
      .pipe(filter((event): event is NavigationEnd => event instanceof NavigationEnd))
      .subscribe((event) => {
        const noHeaderFooterRoutes = [
          '/signin',
          '/register',
          '/login-password',
          '/customer-verification',
          '/new-customer-account',
          '/customer-verify-email',
          '/customer-forgot-password',
          '/404'
        ];

        // Sanitize current URL (remove query params like ?ref=xyz)
        const currentUrl = event.urlAfterRedirects.split('?')[0];

        // 1. Check if the current URL is explicitly listed in our hidden array
        const isExplicitlyHidden = noHeaderFooterRoutes.includes(currentUrl);

        // 2. Check if the current URL does NOT match any valid route defined in app-routing
        const isUnmatchedWildcard = !this.router.config.some((route) => {
          if (route.path === '**' || route.path === '404') return false;
          return '/' + route.path === currentUrl;
        });

        // Hide header and footer if it's explicitly hidden OR landed on an unmatched wildcard
        this.showHeaderFooter = !(isExplicitlyHidden || isUnmatchedWildcard);
      });
  }
}