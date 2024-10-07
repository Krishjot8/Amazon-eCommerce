import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDrawer } from '@angular/material/sidenav';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss']
})
export class NavbarComponent implements OnInit {

  constructor() { }

  @ViewChild('drawer', { static: false }) drawer!: MatDrawer;



  ngOnInit(): void {
  }


  toggleDrawer() {

    console.log('Toggle drawer clicked');
    if (this.drawer) {
      this.drawer.toggle();
    }

  }
}
