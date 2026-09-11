import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-not-found',
  templateUrl: './not-found.component.html',
  styleUrls: ['./not-found.component.scss']
})
export class NotFoundComponent implements OnInit {

   searchTerm: string = '';
    selectedCategory: string = 'all';

  constructor(private router: Router) { }
  dogImages: string[] = [
    'assets/pages/not-found/dogs-of-amazon/Bailey.jpg',
    'assets/pages/not-found/dogs-of-amazon/Barkley&Emma.jpg',
    'assets/pages/not-found/dogs-of-amazon/Bowser.jpg',
    'assets/pages/not-found/dogs-of-amazon/Casey.jpg',
    'assets/pages/not-found/dogs-of-amazon/Clancy.jpg',
    'assets/pages/not-found/dogs-of-amazon/Colby.jpg',
    'assets/pages/not-found/dogs-of-amazon/Duke.jpg',
    'assets/pages/not-found/dogs-of-amazon/Ellie.jpg',
    'assets/pages/not-found/dogs-of-amazon/Frodo.jpg',
    'assets/pages/not-found/dogs-of-amazon/Ike.jpg',
    'assets/pages/not-found/dogs-of-amazon/Jaja.jpg',
    'assets/pages/not-found/dogs-of-amazon/Jax.jpg',
    'assets/pages/not-found/dogs-of-amazon/Lola.jpg',
    'assets/pages/not-found/dogs-of-amazon/Mae.jpg',
    'assets/pages/not-found/dogs-of-amazon/Millie.jpg',
    'assets/pages/not-found/dogs-of-amazon/MissChief.jpg',
    'assets/pages/not-found/dogs-of-amazon/Mollie.jpg',
    'assets/pages/not-found/dogs-of-amazon/Otto.jpg',
    'assets/pages/not-found/dogs-of-amazon/Phoebe.jpg',
    'assets/pages/not-found/dogs-of-amazon/Pixel&Serif.jpg',
    'assets/pages/not-found/dogs-of-amazon/Pretzel.jpg',
    'assets/pages/not-found/dogs-of-amazon/Ranger&Remi.jpg',
    'assets/pages/not-found/dogs-of-amazon/Robin.jpg',
    'assets/pages/not-found/dogs-of-amazon/RoccoDeluca.jpg',
    'assets/pages/not-found/dogs-of-amazon/Rocket.jpg',
    'assets/pages/not-found/dogs-of-amazon/Shadow.jpg',
    'assets/pages/not-found/dogs-of-amazon/Soju.jpg',
    'assets/pages/not-found/dogs-of-amazon/Tanq.jpg',
    'assets/pages/not-found/dogs-of-amazon/Tatula.jpg',
    'assets/pages/not-found/dogs-of-amazon/Waffles.jpg',
    'assets/pages/not-found/dogs-of-amazon/Whiskey.jpg'
  ];
  selectedDogImage: string = '';

  ngOnInit(): void {
  

    const randomIndex = Math.floor(Math.random() * this.dogImages.length);
    this.selectedDogImage = this.dogImages[randomIndex];
  }


  
 onSearch() {
    console.log(
      'Searching for:',
      this.searchTerm,
      'in category:',
      this.selectedCategory
    );

    this.router.navigate([Router], { queryParams: { search: this.searchTerm } });
  }
}
