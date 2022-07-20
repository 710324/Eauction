import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EditPlaceBidComponent } from './edit-place-bid.component';

describe('EditPlaceBidComponent', () => {
  let component: EditPlaceBidComponent;
  let fixture: ComponentFixture<EditPlaceBidComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ EditPlaceBidComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(EditPlaceBidComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
