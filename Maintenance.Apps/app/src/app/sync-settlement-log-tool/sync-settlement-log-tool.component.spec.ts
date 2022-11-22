import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SyncSettlementLogToolComponent } from './sync-settlement-log-tool.component';

describe('SyncSettlementLogToolComponent', () => {
  let component: SyncSettlementLogToolComponent;
  let fixture: ComponentFixture<SyncSettlementLogToolComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SyncSettlementLogToolComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SyncSettlementLogToolComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
