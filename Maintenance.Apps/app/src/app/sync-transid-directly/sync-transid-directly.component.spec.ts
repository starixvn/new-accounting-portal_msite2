import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SyncTransidDirectlyComponent } from './sync-transid-directly.component';

describe('SyncTransidDirectlyComponent', () => {
  let component: SyncTransidDirectlyComponent;
  let fixture: ComponentFixture<SyncTransidDirectlyComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SyncTransidDirectlyComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SyncTransidDirectlyComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
