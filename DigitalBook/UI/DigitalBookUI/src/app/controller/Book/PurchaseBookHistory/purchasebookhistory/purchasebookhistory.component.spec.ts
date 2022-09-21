import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PurchasebookhistoryComponent } from './purchasebookhistory.component';

describe('PurchasebookhistoryComponent', () => {
  let component: PurchasebookhistoryComponent;
  let fixture: ComponentFixture<PurchasebookhistoryComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PurchasebookhistoryComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PurchasebookhistoryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
