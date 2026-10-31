import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ExploreGamingItem } from 'src/app/models/video-games/explore-gaming-item/explore-gaming-item';

@Component({
  selector: 'explore-gaming-carousel',
  templateUrl: './explore-gaming-carousel.component.html',
  styleUrls: ['./explore-gaming-carousel.component.scss'],
})
export class ExploreGamingCarouselComponent implements OnInit {
  currentIndex = 0;

  itemWidth = 215; 
  gap = 15;
  containerWidth = 1150; // Set the container width as needed


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
      title: 'Amazon Game Studios',
      link: '',
      imageUrl:
        'assets/carousel-images/explore-gaming-carousel-images/amazon-game-studios.jpeg',
      isExternal: false,
    },
    {
      title: 'Amazon Games',
      link: 'https://games.amazon.com/en-us',
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
      link: 'https://www.twitch.tv',
      imageUrl:
        'assets/carousel-images/explore-gaming-carousel-images/twitch.jpg',
      isExternal: true,
    },
  ];

  constructor(private router: Router) {}

  ngOnInit(): void {}

 

   get visibleItems(): number {
    return Math.floor(this.containerWidth / (this.itemWidth + this.gap));
  }

  // Prevent scrolling past the exact edge
  get maxIndex(): number {
    return Math.max(0, this.exploreItems.length - this.visibleItems);
  }


  next(): void {
    if (this.currentIndex < this.maxIndex) {
      this.currentIndex = Math.min(
        this.currentIndex + this.visibleItems,
        this.maxIndex
      );
    }
  }

  prev(): void {
    if (this.currentIndex > 0) {
      this.currentIndex = Math.max(
        this.currentIndex - this.visibleItems,
        0
      );
    }
  }

  onItemClick(item: ExploreGamingItem): void {
    if (item.isExternal || item.link.startsWith('http')) {
      window.open(item.link, '_blank');
    } else {
      // Handle internal navigation if needed
     this.router.navigate([item.link]);
    }
  } 

    trackByIndex(index: number, item: ExploreGamingItem): number {
      return index;
    }
  }
