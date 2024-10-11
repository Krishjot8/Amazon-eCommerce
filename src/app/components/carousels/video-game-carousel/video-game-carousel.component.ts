import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'video-game-carousel',
  templateUrl: './video-game-carousel.component.html',
  styleUrls: ['./video-game-carousel.component.scss']
})
export class VideoGameCarouselComponent implements OnInit {

  constructor() { }


brands = [

  {name: 'Playstation',
  imageUrl: 'https://localhost:44366/images/products/Video Games/Brand Images/PlayStation-Logo.jpg'
  },
  {name: 'Xbox',
    imageUrl: 'https://localhost:44366/images/products/Video Games/Brand Images/Xbox-Logo.jpg'
  },
  {name: 'Nintendo',
     imageUrl: 'https://localhost:44366/images/products/Video Games/Brand Images/Nintendo-Logo.jpg'
  }
];

currentIndex = 0;



  ngOnInit(): void { }

  next():  void{

    this.currentIndex = (this.currentIndex + 1) % this.brands.length
  }

  prev(): void {

    this.currentIndex = (this.currentIndex - 1 + this.brands.length) % this.brands.length
  }
}
