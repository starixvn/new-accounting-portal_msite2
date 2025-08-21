import { ComponentFixture, TestBed } from '@angular/core/testing';
// ...existing code...
import { ControlSyncService } from './control-sync-service.service';
import { ConfirmDialogService } from '../shared/components/dialog/dialog.service';
import { MatSlideToggleChange } from '@angular/material/slide-toggle';
import { of } from 'rxjs';
import { CacheKey } from './constants';

import { ControlSyncServiceComponent } from './control-sync-service.component';

describe('ControlSyncServiceComponent', () => {
  let component: ControlSyncServiceComponent;
  let fixture: ComponentFixture<ControlSyncServiceComponent>;
  let mockControlSyncService: any;
  let mockDialogService: any;

  beforeEach(() => {
    mockControlSyncService = {
      getInfoService: jasmine.createSpy('getInfoService').and.returnValue(
        of([
          { queueType: 1, currentStatus: true },
          { queueType: 3, currentStatus: false },
        ])
      ),
      updateStatus: jasmine.createSpy('updateStatus').and.returnValue(of({})),
    };
    mockDialogService = {
      openConfirm: jasmine.createSpy('openConfirm').and.returnValue({
        afterClosed: () => of(true),
      }),
    };

    TestBed.configureTestingModule({
      declarations: [ControlSyncServiceComponent],
      providers: [
        { provide: ControlSyncService, useValue: mockControlSyncService },
        { provide: ConfirmDialogService, useValue: mockDialogService },
      ],
    });
    fixture = TestBed.createComponent(ControlSyncServiceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should load info service on init', () => {
    expect(mockControlSyncService.getInfoService).toHaveBeenCalled();
    expect(component.sportBookLogs.length).toBeGreaterThan(0);
    expect(component.nonSportLogs.length).toBeGreaterThan(0);
    expect(component.sportbookStatus).toBe(true);
    expect(component.nonSportStatus).toBe(false);
  });

  it('should toggle sportBook and nonSport collapse', () => {
    expect(component.collapseSportBook).toBe(false);
    component.toggleSportBook();
    expect(component.collapseSportBook).toBe(true);
    component.toggleSportBook();
    expect(component.collapseSportBook).toBe(false);

    expect(component.collapseNonSport).toBe(false);
    component.toggleNonSport();
    expect(component.collapseNonSport).toBe(true);
    component.toggleNonSport();
    expect(component.collapseNonSport).toBe(false);
  });

  it('should open confirm dialog and handle confirm for sportbook', () => {
    const event = {
      checked: true,
      source: { checked: false },
    } as MatSlideToggleChange;
    spyOn(component, 'handleConfirm');
    component.openConfirmDialog(event);
    expect(mockDialogService.openConfirm).toHaveBeenCalled();
    expect(component.handleConfirm).toHaveBeenCalledWith(
      CacheKey.SportsCacheKey,
      true
    );
  });

  it('should open confirm dialog and handle cancel for sportbook', () => {
    mockDialogService.openConfirm.and.returnValue({
      afterClosed: () => of(false),
    });
    const event = {
      checked: false,
      source: { checked: true },
    } as MatSlideToggleChange;
    component.sportbookStatus = true;
    component.openConfirmDialog(event);
    expect(event.source.checked).toBe(true);
  });

  it('should open confirm dialog and handle confirm for nonSport', () => {
    const event = {
      checked: true,
      source: { checked: false },
    } as MatSlideToggleChange;
    spyOn(component, 'handleConfirm');
    component.openConfirmNonSportDialog(event);
    expect(mockDialogService.openConfirm).toHaveBeenCalled();
    expect(component.handleConfirm).toHaveBeenCalledWith(
      CacheKey.NonSportsCacheKey,
      true
    );
  });

  it('should open confirm dialog and handle cancel for nonSport', () => {
    mockDialogService.openConfirm.and.returnValue({
      afterClosed: () => of(false),
    });
    const event = {
      checked: false,
      source: { checked: true },
    } as MatSlideToggleChange;
    component.nonSportStatus = true;
    component.openConfirmNonSportDialog(event);
    expect(event.source.checked).toBe(true);
  });

  it('should create dialog message for sportbook and nonSport', () => {
    const msgSport = component.createDialogMessage(
      true,
      CacheKey.SportsCacheKey
    );
    expect(msgSport).toContain('START');
    expect(msgSport).toContain('SportBook Sync');
    const msgNonSport = component.createDialogMessage(
      false,
      CacheKey.NonSportsCacheKey
    );
    expect(msgNonSport).toContain('STOP');
    expect(msgNonSport).toContain('NonSport Sync');
  });

  it('should call updateStatus and reload info on handleConfirm', () => {
    spyOn(component, 'loadInfoService');
    component.handleConfirm(CacheKey.SportsCacheKey, true);
    expect(mockControlSyncService.updateStatus).toHaveBeenCalledWith(
      true,
      CacheKey.SportsCacheKey
    );
    expect(component.loadInfoService).toHaveBeenCalled();
  });
});
