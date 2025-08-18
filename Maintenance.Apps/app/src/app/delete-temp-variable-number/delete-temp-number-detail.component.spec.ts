import { ComponentFixture, TestBed } from '@angular/core/testing';
import { DeleteTempVariableNumberDetailComponent } from './delete-temp-number-detail.component';
import { ConfirmDialogService } from '../shared/components/dialog/dialog.service';
import { DeleteTempVariableNumberService } from './delete-temp-variable-number.service';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { of } from 'rxjs';
import { Match } from './match.model';

describe('DeleteTempVariableNumberDetailComponent', () => {
  let component: DeleteTempVariableNumberDetailComponent;
  let fixture: ComponentFixture<DeleteTempVariableNumberDetailComponent>;
  let mockDialogService: any;
  let mockDeleteService: any;
  const mockMatch: Match = {
    matchId: 1,
    serverName: 'DB1',
    eventName: 'Event',
    productName: 'Product',
  } as Match;

  beforeEach(() => {
    mockDialogService = {
      openConfirm: jasmine.createSpy('openConfirm').and.returnValue({
        afterClosed: () => of(true),
      }),
    };
    mockDeleteService = {
      deleteItem: jasmine.createSpy('deleteItem'),
    };

    TestBed.configureTestingModule({
      declarations: [DeleteTempVariableNumberDetailComponent],
      providers: [
        { provide: ConfirmDialogService, useValue: mockDialogService },
        {
          provide: DeleteTempVariableNumberService,
          useValue: mockDeleteService,
        },
        { provide: MAT_DIALOG_DATA, useValue: mockMatch },
      ],
    });
    fixture = TestBed.createComponent(DeleteTempVariableNumberDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should return dataEntries', () => {
    const entries = component.dataEntries;
    expect(Array.isArray(entries)).toBeTrue();
    expect(entries.length).toBeGreaterThan(0);
  });

  it('should open confirm dialog and handle delete', () => {
    spyOn(component, 'handleDelete');
    component.openDeleteConfirmDialog();
    expect(mockDialogService.openConfirm).toHaveBeenCalled();
    expect(component.handleDelete).toHaveBeenCalled();
  });

  it('should not call handleDelete if dialog result is false', () => {
    mockDialogService.openConfirm.and.returnValue({
      afterClosed: () => of(false),
    });
    spyOn(component, 'handleDelete');
    component.openDeleteConfirmDialog();
    expect(component.handleDelete).not.toHaveBeenCalled();
  });

  it('should call deleteItem on handleDelete', () => {
    component.handleDelete();
    expect(mockDeleteService.deleteItem).toHaveBeenCalledWith(mockMatch);
  });
});
