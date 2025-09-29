import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ProductBrand } from 'src/app/models/product-brands/product-brands.model';
import { environment } from 'src/environments/environment.laptop';

@Injectable({
  providedIn: 'root',
})
export class VideoGameCarouselService {
  
  private apiUrl = environment.apiUrl; // automatically picked by Angular

  constructor(private http: HttpClient) { }

  getBrands(): Observable<ProductBrand[]> {
    return this.http.get<ProductBrand[]>(`${this.apiUrl}/brands/video-games`);
  }
}