import { ComponentFixture, TestBed } from '@angular/core/testing';
import { DeleteSettlementLockComponent } from './delete-settlement-lock.component';
import { of } from 'rxjs';
import {
  MatchSettlementLock,
  DeleteSettlementLog,
} from './match-settlement-lock.model';
import { MatchSettlementLockService } from './match-settlement-lock.service';
import { ConfirmDialogService } from '../shared/components/dialog/dialog.service';
import { SnackbarService } from '../shared/services/snackbar.service';

describe('DeleteSettlementLockComponent', () => {
  let component: DeleteSettlementLockComponent;
  let fixture: ComponentFixture<DeleteSettlementLockComponent>;
  let mockMatchService: any;
  let mockDialogService: any;
  let mockSnackbarService: any;

  const mockMatch: MatchSettlementLock = {
    matchId: 1,
    sessionId: 2,
    dbServer: 'DB1',
    totalSettlementProcessing: 10,
    createdTime: '',
    homeTeam: '',
    awayTeam: '',
    serverName: '',
    errorMessage: '',
    messageId: '',
    isReplicated: false,
  };

  beforeEach(() => {
    mockMatchService = {
      getAll: jasmine.createSpy('getAll').and.returnValue(of([mockMatch])),
      deleteItem: jasmine.createSpy('deleteItem').and.returnValue(of({})),
      updateItem: jasmine.createSpy('updateItem').and.returnValue(of({})),
    };
    mockDialogService = {
      openConfirm: jasmine.createSpy('openConfirm').and.returnValue({
        afterClosed: () => of(true),
      }),
    };
    mockSnackbarService = {
      openSuccessSnackBar: jasmine.createSpy('openSuccessSnackBar'),
    };

    TestBed.configureTestingModule({
      declarations: [DeleteSettlementLockComponent],
      providers: [
        { provide: MatchSettlementLockService, useValue: mockMatchService },
        { provide: ConfirmDialogService, useValue: mockDialogService },
        { provide: SnackbarService, useValue: mockSnackbarService },
      ],
    });
    fixture = TestBed.createComponent(DeleteSettlementLockComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should load data on init', () => {
    expect(mockMatchService.getAll).toHaveBeenCalledWith(undefined, 'single');
    expect(component.matches.length).toBe(1);
  });

  it('should call loadData on submit', () => {
    spyOn(component, 'loadData');
    component.submit();
    expect(component.loadData).toHaveBeenCalled();
  });

  it('should update titles on loadData', () => {
    component.typeSettle = 'parlay';
    mockMatchService.getAll.and.returnValue(of([mockMatch]));
    component.loadData();
    expect(component.settlementLockTitle).toBe('ParlaySettlementLock');
    expect(component.settlementProcessingTitle).toBe(
      'ParlaySettlementProcessing'
    );
  });

  it('should open confirm dialog and handle deleteSettlementProcessing', () => {
    spyOn(component, 'deleteSettlementProcessing');
    component.openDeleteConfirmDialog(mockMatch, true);
    expect(mockDialogService.openConfirm).toHaveBeenCalled();
    expect(component.deleteSettlementProcessing).toHaveBeenCalledWith({
      dbServer: mockMatch.dbServer,
      sessionId: mockMatch.sessionId,
      isSettleLock: true,
    });
  });

  it('should not call deleteSettlementProcessing if dialog result is false', () => {
    mockDialogService.openConfirm.and.returnValue({
      afterClosed: () => of(false),
    });
    spyOn(component, 'deleteSettlementProcessing');
    component.openDeleteConfirmDialog(mockMatch, true);
    expect(component.deleteSettlementProcessing).not.toHaveBeenCalled();
  });

  it('should call matchService.deleteItem and reload data on deleteSettlementProcessing', () => {
    spyOn(component, 'loadData');
    const item: DeleteSettlementLog = {
      dbServer: 'DB1',
      sessionId: 2,
      isSettleLock: true,
    };
    component.deleteSettlementProcessing(item);
    expect(mockMatchService.deleteItem).toHaveBeenCalledWith(item, 'single');
    expect(mockSnackbarService.openSuccessSnackBar).toHaveBeenCalled();
    expect(component.loadData).toHaveBeenCalled();
  });

  it('should open confirm dialog and handle updateSettlementLogs', () => {
    spyOn(component, 'updateSettlementLogs');
    component.openUpdateConfirmDialog(mockMatch);
    expect(mockDialogService.openConfirm).toHaveBeenCalled();
    expect(component.updateSettlementLogs).toHaveBeenCalledWith(mockMatch);
  });

  it('should not call updateSettlementLogs if dialog result is false', () => {
    mockDialogService.openConfirm.and.returnValue({
      afterClosed: () => of(false),
    });
    spyOn(component, 'updateSettlementLogs');
    component.openUpdateConfirmDialog(mockMatch);
    expect(component.updateSettlementLogs).not.toHaveBeenCalled();
  });

  it('should call matchService.updateItem and reload data on updateSettlementLogs', () => {
    spyOn(component, 'loadData');
    component.updateSettlementLogs(mockMatch);
    expect(mockMatchService.updateItem).toHaveBeenCalledWith(
      mockMatch,
      'single'
    );
    expect(mockSnackbarService.openSuccessSnackBar).toHaveBeenCalled();
    expect(component.loadData).toHaveBeenCalled();
  });
});
// ...existing code...
