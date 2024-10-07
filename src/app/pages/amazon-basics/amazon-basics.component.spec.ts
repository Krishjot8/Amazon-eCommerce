import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AmazonBasicsComponent } from './amazon-basics.component';

describe('AmazonBasicsComponent', () => {
  let component: AmazonBasicsComponent;
  let fixture: ComponentFixture<AmazonBasicsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AmazonBasicsComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AmazonBasicsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
