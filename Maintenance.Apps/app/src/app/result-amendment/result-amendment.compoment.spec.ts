import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ResultAmendmentCompoment } from './result-amendment.compoment';
import { ConfirmDialogService } from '../shared/components/dialog/dialog.service';
import { ResultAmendmentService } from './result-amendment.service';
import { of } from 'rxjs';
import { ResultAmendment } from './type';

describe('ResultAmendmentCompoment', () => {
  let component: ResultAmendmentCompoment;
  let fixture: ComponentFixture<ResultAmendmentCompoment>;
  let mockDialogService: any;
  let mockResultAmendmentService: any;

  const mockResults: ResultAmendment[] = [
    { selected: true } as ResultAmendment,
    { selected: false } as ResultAmendment,
  ];

  beforeEach(() => {
    mockDialogService = {
      openConfirm: jasmine.createSpy('openConfirm').and.returnValue({
        afterClosed: () => of(true),
      }),
      openWarning: jasmine.createSpy('openWarning'),
    };
    mockResultAmendmentService = {
      getAmenentResults: jasmine
        .createSpy('getAmenentResults')
        .and.returnValue(of(mockResults)),
      updateStatus: jasmine.createSpy('updateStatus').and.returnValue(of({})),
    };

    TestBed.configureTestingModule({
      declarations: [ResultAmendmentCompoment],
      providers: [
        { provide: ConfirmDialogService, useValue: mockDialogService },
        {
          provide: ResultAmendmentService,
          useValue: mockResultAmendmentService,
        },
      ],
    });
    fixture = TestBed.createComponent(ResultAmendmentCompoment);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should load data on init', () => {
    expect(mockResultAmendmentService.getAmenentResults).toHaveBeenCalledWith({
      matchId: null,
      statusId: 0,
    });
    expect(component.results).toEqual(mockResults);
  });

  it('should call loadData on submit', () => {
    spyOn(component, 'loadData');
    component.submit();
    expect(component.loadData).toHaveBeenCalled();
  });

  it('should open warning dialog if no selected results', () => {
    component.results = [{ selected: false } as ResultAmendment];
    component.openConfirmDialog();
    expect(mockDialogService.openWarning).toHaveBeenCalled();
    expect(mockDialogService.openConfirm).not.toHaveBeenCalled();
  });

  it('should open confirm dialog and handle change status', () => {
    spyOn(component as any, 'handleChangeStatus');
    component.results = [{ selected: true } as ResultAmendment];
    component.openConfirmDialog();
    expect(mockDialogService.openConfirm).toHaveBeenCalled();
    expect((component as any).handleChangeStatus).toHaveBeenCalledWith([
      { selected: true },
    ]);
  });

  it('should call updateStatus and reload data on handleChangeStatus', () => {
    spyOn(component, 'loadData');
    const selectedResults = [{ selected: true } as ResultAmendment];
    (component as any).handleChangeStatus(selectedResults);
    expect(mockResultAmendmentService.updateStatus).toHaveBeenCalledWith({
      results: selectedResults,
    });
    expect(component.loadData).toHaveBeenCalled();
  });
});
