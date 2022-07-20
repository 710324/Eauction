import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PlaceBidsComponent } from './place-bids.component';

describe('PlaceBidsComponent', () => {
  let component: PlaceBidsComponent;
  let fixture: ComponentFixture<PlaceBidsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PlaceBidsComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PlaceBidsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
