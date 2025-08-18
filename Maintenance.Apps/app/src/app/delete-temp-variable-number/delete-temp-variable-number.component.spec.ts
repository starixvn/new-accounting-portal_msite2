import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DeleteTempVariableNumberComponent } from './delete-temp-variable-number.component';

import { of, Subject } from 'rxjs';
import { Match } from './match.model';
import { DeleteTempVariableNumberDetailComponent } from './delete-temp-number-detail.component';
import { MatDialog } from '@angular/material/dialog';
import { DeleteTempVariableNumberService } from './delete-temp-variable-number.service';

const mockMatch: Match = {
  matchId: 1,
  serverName: 'DB1',
  eventName: 'Event',
  productName: 'Product',
} as Match;

describe('DeleteTempVariableNumberComponent', () => {
  let component: DeleteTempVariableNumberComponent;
  let fixture: ComponentFixture<DeleteTempVariableNumberComponent>;
  let mockDialog: any;
  let mockService: any;
  let dataUpdate$: Subject<void>;

  beforeEach(() => {
    dataUpdate$ = new Subject<void>();
    mockDialog = {
      open: jasmine.createSpy('open'),
      closeAll: jasmine.createSpy('closeAll'),
    };
    mockService = {
      getAll: jasmine.createSpy('getAll').and.returnValue(of([mockMatch])),
      getDetail: jasmine.createSpy('getDetail').and.returnValue(of(mockMatch)),
      dataUpdate$: dataUpdate$,
    };

    TestBed.configureTestingModule({
      declarations: [DeleteTempVariableNumberComponent],
      providers: [
        { provide: MatDialog, useValue: mockDialog },
        { provide: DeleteTempVariableNumberService, useValue: mockService },
      ],
    });
    fixture = TestBed.createComponent(DeleteTempVariableNumberComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should load data on init when dataUpdate$ emits', () => {
    spyOn(component, 'loadData');
    dataUpdate$.next();
    expect(component.loadData).toHaveBeenCalled();
    expect(mockDialog.closeAll).toHaveBeenCalled();
  });

  it('should load matches on loadData', () => {
    component.matches = [];
    component.loadData();
    expect(mockService.getAll).toHaveBeenCalledWith(undefined);
    expect(component.matches).toEqual([mockMatch]);
  });

  it('should call loadData on submit', () => {
    spyOn(component, 'loadData');
    component.submit();
    expect(component.loadData).toHaveBeenCalled();
  });

  it('should open detail dialog with correct config', async () => {
    await component.openDetailMatchDialog(mockMatch);
    expect(mockService.getDetail).toHaveBeenCalledWith(mockMatch);
    expect(mockDialog.open).toHaveBeenCalledWith(
      DeleteTempVariableNumberDetailComponent,
      {
        data: mockMatch,
        width: '600px',
        panelClass: 'temp-number-dialog',
      }
    );
  });
});
// ...existing code...
