import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NintendoHomeComponent } from './home.component';

describe('NintendoHomeComponent', () => {
  let component: NintendoHomeComponent;
  let fixture: ComponentFixture<NintendoHomeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ NintendoHomeComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(NintendoHomeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
