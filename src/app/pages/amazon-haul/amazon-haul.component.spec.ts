import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AmazonHaulComponent } from './amazon-haul.component';

describe('AmazonHaulComponent', () => {
  let component: AmazonHaulComponent;
  let fixture: ComponentFixture<AmazonHaulComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AmazonHaulComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AmazonHaulComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
