import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, Observable } from 'rxjs';
import { ProductBrand } from 'src/app/models/products/product-brand.model';

import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root',
})
export class TopBrandsCarouselService {

  private apiUrl = environment.apiUrl; // automatically picked by Angular

  constructor(private http: HttpClient) { }

  getBrands(): Observable<ProductBrand[]> {
  // Strip '/api' from the end of the base URL so it points to the root (e.g., https://localhost:4366)
  const baseUrl = this.apiUrl.replace(/\/api\/?$/, '');

  return this.http.get<any[]>(`${this.apiUrl}/productbrands`).pipe(
    map(data =>
      data.map((brand, index) => {
        // Handle potential case differences (logoUrl vs LogoUrl) and clean up leading slashes
        const rawLogoUrl = brand.logoUrl || brand.LogoUrl || '';
        const cleanLogoPath = rawLogoUrl.replace(/^\//, '');

        return {
          id: brand.id ?? brand.Id ?? index + 1,
          name: brand.name || brand.productBrand || brand.Name || '',
          logoUrl: `${baseUrl}/${cleanLogoPath}`
        };
      })
    )
  );
}


}
