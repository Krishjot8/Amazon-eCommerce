import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDrawer } from '@angular/material/sidenav';
import { Router } from '@angular/router';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss']
})
export class NavbarComponent implements OnInit {

  username: string | null = null;


 ngOnInit(): void {
  this.username = localStorage.getItem('username');

  }


showNavbar = true;



  constructor(private router: Router) {}

  @ViewChild('drawer', { static: false }) drawer!: MatDrawer;


  toggleDrawer() {

    console.log('Toggle drawer clicked');
    if (this.drawer) {
      this.drawer.toggle();
    }

  }
}
