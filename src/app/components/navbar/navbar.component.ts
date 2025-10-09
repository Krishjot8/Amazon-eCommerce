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

  isActive: boolean = false;
  searchTerm: string = '';
  selectedCategory: string = 'all';
  showDepartments: boolean = false;
  username: string | null = null;

  constructor(private router: Router) {}

  showNavbar = true;

  ngOnInit(): void {
    this.username = localStorage.getItem('username');
  }

  //departments

  departments = [
    { value: 'all-departments', label: 'All Departments' },
    { value: 'alexa-skills', label: 'Alexa Skills' },
    { value: 'amazon-autos', label: 'Amazon Autos' },
    { value: 'amazon-devices', label: 'Amazon Devices' },
    { value: 'amazon-fresh', label: 'Amazon Fresh' },
    { value: 'amazon-global-store', label: 'Amazon Global Store' },
    { value: 'amazon-haul', label: 'Amazon Haul' },
    { value: 'amazon-one-medical', label: 'Amazon One Medical' },
    { value: 'amazon-pharmacy', label: 'Amazon Pharmacy' },
    { value: 'apps-games', label: 'Apps & Games' },
    { value: 'arts-crafts-sewing', label: 'Arts, Crafts & Sewing' },
    { value: 'audible-books', label: 'Audible Books & Originals' },
    { value: 'automotive-parts', label: 'Automotive Parts & Accessories' },
    { value: 'beauty-personal', label: 'Beauty & Personal Care' },
    { value: 'cds-vinyl', label: 'CDs & Vinyl' },
    { value: 'phones-accessories', label: 'Cell Phones & Accessories' },
    { value: 'clothing-shoes-jewlery', label: 'Clothing, Shoes & Jewlery' },
    {
      value: 'women-clothing-shoes-jewlery',
      label: `Women's Clothing, Shoes & Jewlery`,
    },
    {
      value: 'men-clothing-shoes-jewlery',
      label: `Men's Clothing, Shoes & Jewlery`,
    },
    {
      value: 'girl-clothing-shoes-jewlery',
      label: `Girl's Clothing, Shoes & Jewlery`,
    },
    {
      value: 'boy-clothing-shoes-jewlery',
      label: `Boy's Clothing, Shoes & Jewlery`,
    },
    {
      value: 'baby-clothing-shoes-jewlery',
      label: `Baby Clothing, Shoes & Jewlery`,
    },
    { value: 'collectibles-fine-art', label: 'Collectibles & Fine Art' },
    { value: 'computers', label: 'Computers' },
    { value: 'credit-payment-cards', label: 'Credit & Payment Cards' },
    { value: 'digital-music', label: 'Digital Music' },
    { value: 'electronics', label: 'Electronics' },
    { value: 'garden-outdoor', label: 'Garden & Outdoor' },
    { value: 'gift-cards', label: 'Gift Cards' },
    { value: 'grocery-gourmet-food', label: 'Grocery & Gourmet Food' },
    { value: 'handmade', label: 'Handmade' },
    {
      value: 'health-household-baby-care',
      label: 'Health, Household & Baby Care',
    },
    { value: 'home-business-services', label: 'Home & Business Services' },
    { value: 'home-kitchen', label: 'Home & Kitchen' },
    { value: 'industrial-scientific', label: 'Industrial & Scientific' },
    { value: 'just-for-prime', label: 'Just for Prime' },
    { value: 'amazon-kindle', label: 'Kindle Store' },
    { value: 'luggage-travel-gear', label: 'Luggage & Travel Gear' },
    { value: 'luxury-stores', label: 'Luxury Stores' },
    { value: 'magazine-subscriptions', label: 'Magazine Subscriptions' },
    { value: 'movies-tv', label: 'Movies & TV' },
    { value: 'musical-instruments', label: 'Musical Instruments' },
    { value: 'office-products', label: 'Office Products' },
    { value: 'pet-supplies', label: 'Pet Supplies' },
    { value: 'premium-beauty', label: 'Premium Beauty' },
    { value: 'prime-video', label: 'Prime Video' },
    { value: 'same-day-store', label: 'Same-Day Store' },
    { value: 'smart-home', label: 'Smart Home' },
    { value: 'software', label: 'Software' },
    { value: 'sports-outdoors', label: 'Sports & Outdoors' },
    { value: 'subscribe-save', label: 'Subscribe & Save' },
    { value: 'subscription-boxes', label: 'Subscription Boxes' },
    { value: 'tools-home-improvement', label: 'Tools & Home Improvement' },
    { value: 'toys-games', label: 'Toys & Games' },
    { value: 'under-ten-dollars', label: 'Under $10' },
    { value: 'video-games', label: 'Video Games' },
    { value: 'whole-foods-market', label: 'Whole Foods Market' },
  ];

  categoryRoutes: { [key: string]: string } = {
    'all-departments': '/',
    fashion: 'amazon-fashion',
    'video-games': '/video-games',
  };

  onSearch() {
    console.log(
      'Searching for:',
      this.searchTerm,
      'in category:',
      this.selectedCategory
    );

    const route = this.categoryRoutes[this.selectedCategory] || '/search';
    this.router.navigate([route], { queryParams: { search: this.searchTerm } });
  }

  toggleActive() {
    this.isActive = !this.isActive;
  }

  toggleDrawer() {
    console.log('Toggle drawer clicked');
    if (this.drawer) {
      this.drawer.toggle();
    }
  }
}
