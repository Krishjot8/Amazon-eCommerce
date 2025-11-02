import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TopBrandsCarouselComponent } from './top-brands-carousel.component';

describe('TopBrandsCarouselComponent', () => {
  let component: TopBrandsCarouselComponent;
  let fixture: ComponentFixture<TopBrandsCarouselComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TopBrandsCarouselComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TopBrandsCarouselComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
