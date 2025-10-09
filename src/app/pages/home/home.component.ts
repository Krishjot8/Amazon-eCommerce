import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
})
export class HomeComponent implements OnInit {
  images: string[] = [
    'assets/carousel-images/home-component-carousel-images/nba-prime.jpg',
    'assets/carousel-images/home-component-carousel-images/amazon-pharmacy.jpg',
    'assets/carousel-images/home-component-carousel-images/early-kitchen-deals.jpg',
    'assets/carousel-images/home-component-carousel-images/gen-v.jpg',
        'assets/carousel-images/home-component-carousel-images/halloween-costumes.jpg',
  ];

  duplicatedImages: string[] = [];
  currentIndex = 0;
  transition = 'transform 0.2s ease-in-out';
  autoSlideInterval: any;

  ngOnInit() {
    // Duplicate images for seamless looping
    this.duplicatedImages = [...this.images, ...this.images];

    // Auto slide every 4 seconds
    this.autoSlideInterval = setInterval(() => this.nextSlide(), 9000);
  }

  nextSlide() {
    this.currentIndex++;
    this.transition = 'transform 0.3s ease-in-out';
  }

  prevSlide() {
    this.currentIndex--;
    this.transition = 'transform 0.3s ease-in-out';
  }

  // Called after CSS transition ends
  onTransitionEnd() {
    const total = this.images.length;

    if (this.currentIndex >= total) {
      // If we reached the duplicate end, reset to the start instantly
      this.transition = 'none';
      this.currentIndex = 0;
    }

    if (this.currentIndex < 0) {
      // If going before start, jump to the last duplicate instantly
      this.transition = 'none';
      this.currentIndex = total - 1;
    }
  }
}
