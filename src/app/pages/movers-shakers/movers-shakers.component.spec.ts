import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MoversShakersComponent } from './movers-shakers.component';

describe('MoversShakersComponent', () => {
  let component: MoversShakersComponent;
  let fixture: ComponentFixture<MoversShakersComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ MoversShakersComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MoversShakersComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
