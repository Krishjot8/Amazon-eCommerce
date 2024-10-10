import { ComponentFixture, TestBed } from '@angular/core/testing';

import { VideoGameCarouselComponent } from './video-game-carousel.component';

describe('VideoGameCarouselComponent', () => {
  let component: VideoGameCarouselComponent;
  let fixture: ComponentFixture<VideoGameCarouselComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ VideoGameCarouselComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(VideoGameCarouselComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
