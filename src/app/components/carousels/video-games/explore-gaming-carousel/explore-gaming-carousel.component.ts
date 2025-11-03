import { Component, OnInit } from '@angular/core';
import { ExploreGamingItem } from 'src/app/models/video-games/explore-gaming-item/explore-gaming-item';

@Component({
  selector: 'explore-gaming-carousel',
  templateUrl: './explore-gaming-carousel.component.html',
  styleUrls: ['./explore-gaming-carousel.component.scss'],
})
export class ExploreGamingCarouselComponent implements OnInit {
  currentIndex = 0;

  exploreItems: ExploreGamingItem[] = [
    {
      title: 'Secret Level on Prime Video',
      link: '',
      imageUrl:
        'assets/carousel-images/explore-gaming-carousel-images/secret-level.jpg',
      isExternal: false,
    },

    {
      title: 'Fallout on Prime Video',
      link: '',
      imageUrl:
        'assets/carousel-images/explore-gaming-carousel-images/fallout-on-prime-video.jpg',
      isExternal: false,
    },

    {
      title: 'Amazon Games',
      link: '',
      imageUrl:
        'assets/carousel-images/explore-gaming-carousel-images/amazon-games.jpg',
      isExternal: false,
    },

    {
      title: 'Luna',
      link: '',
      imageUrl:
        'assets/carousel-images/explore-gaming-carousel-images/luna.jpg',
      isExternal: false,
    },

    {
      title: 'Prime Gaming',
      link: '',
      imageUrl:
        'assets/carousel-images/explore-gaming-carousel-images/prime-gaming.jpg',
      isExternal: false,
    },

    {
      title: 'Renewed',
      link: '',
      imageUrl:
        'assets/carousel-images/explore-gaming-carousel-images/amazon-renewed.jpg',
      isExternal: false,
    },

    {
      title: 'Twitch',
      link: '',
      imageUrl:
        'assets/carousel-images/explore-gaming-carousel-images/twitch.jpg',
      isExternal: true,
    },
  ];

  constructor() {}

  ngOnInit(): void {}

  next(): void {
    this.currentIndex = (this.currentIndex + 1) % this.exploreItems.length;
  }

  prev(): void {
    this.currentIndex =
      (this.currentIndex - 1 + this.exploreItems.length) %
      this.exploreItems.length;
  }
}
