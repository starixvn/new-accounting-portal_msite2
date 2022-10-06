import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DeleteParlayLockingComponent } from './delete-parlay-locking.component';

describe('DeleteParlayLockingComponent', () => {
  let component: DeleteParlayLockingComponent;
  let fixture: ComponentFixture<DeleteParlayLockingComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DeleteParlayLockingComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DeleteParlayLockingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
