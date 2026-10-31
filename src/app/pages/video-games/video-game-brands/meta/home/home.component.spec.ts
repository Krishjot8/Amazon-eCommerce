import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MetaHomeComponent } from './home.component';

describe('MetaHomeComponent', () => {
  let component: MetaHomeComponent;
  let fixture: ComponentFixture<MetaHomeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ MetaHomeComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MetaHomeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
