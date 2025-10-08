import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDrawer } from '@angular/material/sidenav';
import { Router } from '@angular/router';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss'],
})
export class NavbarComponent implements OnInit {
  @ViewChild('drawer', { static: false }) drawer!: MatDrawer;

  searchTerm: string = '';
  selectedCategory: string = 'all';

  username: string | null = null;

  constructor(private router: Router) {}

  showNavbar = true;

  ngOnInit(): void {
    this.username = localStorage.getItem('username');
  }

  onSearch() {
    console.log(
      'Searching for:',
      this.searchTerm,
      'in category:',
      this.selectedCategory
    );

    switch (this.selectedCategory) {
      case 'Video Games':
        this.router.navigate(['/video-games'], {
          queryParams: { search: this.searchTerm },
        });
        break;

        case 'Fashion':
          this.router.navigate(['/amazon-fashion'], { 
            queryParams: { search: this.searchTerm }
           });
           break;
    }
  }

  toggleDrawer() {
    console.log('Toggle drawer clicked');
    if (this.drawer) {
      this.drawer.toggle();
    }
  }
}
