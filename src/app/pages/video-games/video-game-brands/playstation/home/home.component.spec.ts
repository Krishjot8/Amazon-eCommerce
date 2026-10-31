import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PlaystationHomeComponent } from './home.component';

describe('PlaystationHomeComponent', () => {
  let component: PlaystationHomeComponent;
  let fixture: ComponentFixture<PlaystationHomeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [PlaystationHomeComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(PlaystationHomeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
