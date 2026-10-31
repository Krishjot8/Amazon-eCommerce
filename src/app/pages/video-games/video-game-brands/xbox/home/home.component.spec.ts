import { ComponentFixture, TestBed } from '@angular/core/testing';

import { XboxHomeComponent } from './home.component';

describe('XboxHomeComponent', () => {
  let component: XboxHomeComponent;
  let fixture: ComponentFixture<XboxHomeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ XboxHomeComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(XboxHomeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
