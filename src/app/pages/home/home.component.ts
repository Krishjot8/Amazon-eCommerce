import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {

  images: string[] = [
'assets/carousel-images/home-component-carousel-images/see-what-we-launched.jpg',
'assets/carousel-images/home-component-carousel-images/nba-prime.jpg',
'assets/carousel-images/home-component-carousel-images/nastygal.jpg',
'assets/carousel-images/home-component-carousel-images/hotel-costiera-prime-series.jpg',
'assets/carousel-images/home-component-carousel-images/amazon-pharmacy.jpg'

  ]

currentIndex = 0;

  ngOnInit(): void {
    setInterval(() => this.nextSlide(), 6000);
  }

nextSlide(){
this.currentIndex = (this.currentIndex + 1) % this.images.length;
}


prevSlide(){

  this.currentIndex = (this.currentIndex - 1 + this.images.length) % this.images.length;

}

}
