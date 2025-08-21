import { TestBed } from '@angular/core/testing';
import { ConfirmDialogService } from './dialog.service';
import { MatDialog } from '@angular/material/dialog';
import { ConfirmDialogComponent } from './confirm/confirm-dialog.component';
import { WarningDialogComponent } from './warning/warning-dialog.component';

describe('ConfirmDialogService', () => {
  let service: ConfirmDialogService;
  let matDialogSpy: jasmine.SpyObj<MatDialog>;

  beforeEach(() => {
    matDialogSpy = jasmine.createSpyObj('MatDialog', ['open']);
    TestBed.configureTestingModule({
      providers: [
        ConfirmDialogService,
        { provide: MatDialog, useValue: matDialogSpy },
      ],
    });
    service = TestBed.inject(ConfirmDialogService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should open ConfirmDialogComponent with correct data', () => {
    const dialogData = {
      message: 'msg',
      title: 'title',
      confirmText: 'ok',
      cancelText: 'cancel',
      confirmStyle: 'style',
    };
    service.openConfirm(dialogData);
    expect(matDialogSpy.open).toHaveBeenCalledWith(ConfirmDialogComponent, {
      data: dialogData,
      width: '400px',
      disableClose: true,
      panelClass: 'confirm-dialog',
    });
  });

  it('should open WarningDialogComponent with correct data', () => {
    const warningData = { message: 'warn' };
    service.openWarning(warningData);
    expect(matDialogSpy.open).toHaveBeenCalledWith(WarningDialogComponent, {
      data: warningData,
      width: '400px',
      disableClose: true,
      panelClass: 'warning-dialog',
    });
  });
});
