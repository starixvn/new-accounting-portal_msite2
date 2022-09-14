import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DeleteSettlementLockComponent } from './delete-settlement-lock.component';

describe('DeleteSettlementLockComponent', () => {
  let component: DeleteSettlementLockComponent;
  let fixture: ComponentFixture<DeleteSettlementLockComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DeleteSettlementLockComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DeleteSettlementLockComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
