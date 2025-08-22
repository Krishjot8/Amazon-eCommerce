import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SellerVerifyEmailComponent } from './seller-verify-email.component';

describe('SellerVerifyEmailComponent', () => {
  let component: SellerVerifyEmailComponent;
  let fixture: ComponentFixture<SellerVerifyEmailComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SellerVerifyEmailComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SellerVerifyEmailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
