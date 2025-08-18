import { ComponentFixture, TestBed } from '@angular/core/testing';
import { DeleteParlayLockingComponent } from './delete-parlay-locking.component';
import { MatchParlayLockingService } from './delete-parlay-locking.service';
import { ConfirmDialogService } from '../shared/components/dialog/dialog.service';
import { of } from 'rxjs';
import {
  ParlayLockingRequest,
  MatchParlayLocking,
} from './match-parlay-locking.model';

const mockMatches: MatchParlayLocking[] = [
  {
    serverName: 'DB1',
    matchId: 123,
    totalAffectedRefno: 5,
    session: 'S1',
  } as MatchParlayLocking,
];

describe('DeleteParlayLockingComponent', () => {
  let component: DeleteParlayLockingComponent;
  let fixture: ComponentFixture<DeleteParlayLockingComponent>;
  let mockMatchService: any;
  let mockDialogService: any;

  beforeEach(() => {
    mockMatchService = {
      getAll: jasmine.createSpy('getAll').and.returnValue(of(mockMatches)),
      deleteItem: jasmine.createSpy('deleteItem').and.returnValue(of({})),
    };
    mockDialogService = {
      openConfirm: jasmine.createSpy('openConfirm').and.returnValue({
        afterClosed: () => of(true),
      }),
    };

    TestBed.configureTestingModule({
      declarations: [DeleteParlayLockingComponent],
      providers: [
        { provide: MatchParlayLockingService, useValue: mockMatchService },
        { provide: ConfirmDialogService, useValue: mockDialogService },
      ],
    });
    fixture = TestBed.createComponent(DeleteParlayLockingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should load data on init', () => {
    expect(mockMatchService.getAll).toHaveBeenCalledWith({ timeRange: 1 });
    expect(component.matches).toEqual(mockMatches);
  });

  it('should call loadData on submit', () => {
    spyOn(component, 'loadData');
    component.submit();
    expect(component.loadData).toHaveBeenCalled();
  });

  it('should update matches on loadData', () => {
    component.matches = [];
    component.loadData();
    expect(mockMatchService.getAll).toHaveBeenCalledWith(component.filter);
    expect(component.matches).toEqual(mockMatches);
  });

  it('should open confirm dialog and handle delete', () => {
    spyOn(component, 'handleDeleteItem');
    component.openDeleteConfirmDialog(mockMatches[0]);
    expect(mockDialogService.openConfirm).toHaveBeenCalled();
    expect(component.handleDeleteItem).toHaveBeenCalledWith(mockMatches[0]);
  });

  it('should not delete if dialog result is false', () => {
    mockDialogService.openConfirm.and.returnValue({
      afterClosed: () => of(false),
    });
    spyOn(component, 'handleDeleteItem');
    component.openDeleteConfirmDialog(mockMatches[0]);
    expect(component.handleDeleteItem).not.toHaveBeenCalled();
  });

  it('should call deleteItem and reload data on handleDeleteItem', () => {
    spyOn(component, 'loadData');
    component.handleDeleteItem(mockMatches[0]);
    expect(mockMatchService.deleteItem).toHaveBeenCalledWith(mockMatches[0]);
    expect(component.loadData).toHaveBeenCalled();
  });
});
