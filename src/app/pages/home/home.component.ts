import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
})
export class HomeComponent implements OnInit {
  images: string[] = [
    'assets/carousel-images/home-component-carousel-images/see-what-we-launched.jpg',
    'assets/carousel-images/home-component-carousel-images/nba-prime.jpg',
    'assets/carousel-images/home-component-carousel-images/nastygal.jpg',
    'assets/carousel-images/home-component-carousel-images/hotel-costiera-prime-series.jpg',
'assets/carousel-images/home-component-carousel-images/amazon-pharmacy.jpg',
'assets/carousel-images/home-component-carousel-images/prime-big-deal-days.jpg'

  ];

  duplicatedImages: string[] = [];
  currentIndex = 0;
  transition = 'transform 0.8s ease-in-out';
  autoSlideInterval: any;

  ngOnInit() {
    // Duplicate images for seamless looping
    this.duplicatedImages = [...this.images, ...this.images];

    // Auto slide every 4 seconds
    this.autoSlideInterval = setInterval(() => this.nextSlide(), 4000);
  }

  nextSlide() {
    this.currentIndex++;
    this.transition = 'transform 0.8s ease-in-out';
  }

  prevSlide() {
    this.currentIndex--;
    this.transition = 'transform 0.8s ease-in-out';
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
