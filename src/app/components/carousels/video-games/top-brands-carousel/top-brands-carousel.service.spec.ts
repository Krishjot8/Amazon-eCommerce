import { TestBed } from '@angular/core/testing';

import { TopBrandsCarouselService } from './top-brands-carousel.service';

describe('TopBrandsCarouselService', () => {
  let service: TopBrandsCarouselService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(TopBrandsCarouselService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
