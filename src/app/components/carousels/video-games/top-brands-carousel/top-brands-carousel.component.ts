import { Component, OnInit } from '@angular/core';
import { ProductBrand } from 'src/app/models/products/product-brand.model';
import { TopBrandsCarouselService } from './top-brands-carousel.service';

@Component({
  selector: 'top-brands-carousel',
  templateUrl: './top-brands-carousel.component.html',
  styleUrls: ['./top-brands-carousel.component.scss'],
})
export class TopBrandsCarouselComponent implements OnInit {
  brands: ProductBrand[] = [];
  currentIndex = 0;
  itemWidth = 215; 
  gap = 15;
  containerWidth = 1150; // Set the container width as needed

  constructor(private brandService: TopBrandsCarouselService) {}

  ngOnInit(): void {
    this.getBrands();
  }

  getBrands(): void {
    this.brandService.getBrands().subscribe({
      next: (data) => {
        this.brands = data;
      },
      error: (err) => {
        console.error('Failed to load brands:', err);
      },
    });
  }

  get visibleItems(): number {
    return Math.floor(this.containerWidth / (this.itemWidth + this.gap));
  }

  // Prevent scrolling past the exact edge
  get maxIndex(): number {
    return Math.max(0, this.brands.length - this.visibleItems);
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

  trackByBrandId(index: number, brand: ProductBrand): number {
    return brand.id ?? index;
  }
}