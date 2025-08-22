import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BusinessVerifyEmailComponent } from './business-verify-email.component';

describe('BusinessVerifyEmailComponent', () => {
  let component: BusinessVerifyEmailComponent;
  let fixture: ComponentFixture<BusinessVerifyEmailComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ BusinessVerifyEmailComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(BusinessVerifyEmailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
