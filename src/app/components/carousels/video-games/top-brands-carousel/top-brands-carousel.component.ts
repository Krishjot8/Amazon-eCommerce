import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { ProductBrand } from 'src/app/models/products/product-brand.model';
import { environment } from 'src/environments/environment';
@Component({
  selector: 'top-brands-carousel',
  templateUrl: './top-brands-carousel.component.html',
  styleUrls: ['./top-brands-carousel.component.scss']
})
export class TopBrandsCarouselComponent implements OnInit {

  brands: ProductBrand[] = [];
  currentIndex = 0;
  itemWidth = 215;
  gap = 15;
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
        logoUrl: `${environment.apiUrl.replace('/api', '')}${brand.logoUrl}`
      }))
    })
  }

 next(): void{
const visibleItems = Math.floor(1150/this.itemWidth);
if(this.currentIndex < this.brands.length - visibleItems){

this.currentIndex++;
}

 }


 prev(): void {
if(this.currentIndex > 0){
  this.currentIndex--;
}

 }

 trackByBrandId(index: number, brand: ProductBrand): number {
  return brand.id ?? index;
}
}
