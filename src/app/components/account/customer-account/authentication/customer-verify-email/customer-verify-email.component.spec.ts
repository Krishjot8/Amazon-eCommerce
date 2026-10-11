import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CustomerVerifyEmailComponent } from './customer-verify-email.component';

describe('CustomerVerifyEmailComponent', () => {
  let component: CustomerVerifyEmailComponent;
  let fixture: ComponentFixture<CustomerVerifyEmailComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CustomerVerifyEmailComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CustomerVerifyEmailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
