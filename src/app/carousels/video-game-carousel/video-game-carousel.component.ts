import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'video-game-carousel',
  templateUrl: './video-game-carousel.component.html',
  styleUrls: ['./video-game-carousel.component.scss']
})
export class VideoGameCarouselComponent implements OnInit {

  constructor() { }


games = [

  {name: 'Playstation', imageUrl: ``},
  {name: 'Xbox', imageUrl: ``},
  {name: 'Nintendo', imageUrl: ``}
];

currentIndex = 0;



  ngOnInit(): void { }

  next():  void{

    this.currentIndex = (this.currentIndex + 1) % this.games.length
  }

  prev(): void {

    this.currentIndex = (this.currentIndex - 1 + this.games.length) % this.games.length
  }
}
