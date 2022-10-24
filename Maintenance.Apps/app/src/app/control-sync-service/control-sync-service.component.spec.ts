import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ControlSyncServiceComponent } from './control-sync-service.component';

describe('ControlSynServiceComponent', () => {
  let component: ControlSyncServiceComponent;
  let fixture: ComponentFixture<ControlSyncServiceComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ControlSyncServiceComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(ControlSyncServiceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
