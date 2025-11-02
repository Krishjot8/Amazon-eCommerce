import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ExploreGamingCarouselComponent } from './explore-gaming-carousel.component';

describe('ExploreGamingCarouselComponent', () => {
  let component: ExploreGamingCarouselComponent;
  let fixture: ComponentFixture<ExploreGamingCarouselComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ExploreGamingCarouselComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ExploreGamingCarouselComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
