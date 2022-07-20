import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ViewProductBidsComponent } from './view-product-bids.component';

describe('ViewProductBidsComponent', () => {
  let component: ViewProductBidsComponent;
  let fixture: ComponentFixture<ViewProductBidsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ViewProductBidsComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ViewProductBidsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
