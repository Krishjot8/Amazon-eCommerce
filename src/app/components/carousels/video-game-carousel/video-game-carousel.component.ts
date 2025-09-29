import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { ProductBrand } from 'src/app/models/product-brands/product-brands.model';
import { environment } from 'src/environments/environment.laptop';

@Component({
  selector: 'video-game-carousel',
  templateUrl: './video-game-carousel.component.html',
  styleUrls: ['./video-game-carousel.component.scss']
})
export class VideoGameCarouselComponent implements OnInit {

 

  brands: ProductBrand[] = [];
  currentIndex = 0;
  constructor(private http: HttpClient) {}

  ngOnInit(): void{
this.getBrands();

  }
  //
  getBrands(): void{
    this.http.get<ProductBrand[]>(`${environment.apiUrl}/productbrands`).subscribe(data =>{

      this.brands = data.map((brand, index) =>({
// If backend does not have ID, assign temporary frontend ID
        id: brand.id ?? index + 1,
        name: brand.name,
        pictureUrl: brand.pictureUrl
      }))
    })
  }

 next(): void{

this.currentIndex = (this.currentIndex + 1) % this.brands.length;
 }


 prev(): void {
this.currentIndex = (this.currentIndex - 1 + this.brands.length) % this.brands.length

 }

 trackByBrandId(index: number, brand: ProductBrand): number {
  return brand.id;
}
}
