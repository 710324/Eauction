import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NewBuyerComponent } from './new-buyer.component';

describe('NewBuyerComponent', () => {
  let component: NewBuyerComponent;
  let fixture: ComponentFixture<NewBuyerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ NewBuyerComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(NewBuyerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
