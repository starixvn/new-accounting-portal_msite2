import { SnackbarService } from './snackbar.service';
import { MatSnackBar } from '@angular/material/snack-bar';

describe('SnackbarService', () => {
  let service: SnackbarService;
  let matSnackBarSpy: jasmine.SpyObj<MatSnackBar>;

  beforeEach(() => {
    matSnackBarSpy = jasmine.createSpyObj('MatSnackBar', ['open']);
    service = new SnackbarService(matSnackBarSpy);
  });

  it('should open success snackbar with default message', () => {
    service.openSuccessSnackBar();
    expect(matSnackBarSpy.open).toHaveBeenCalledWith(
      'Successful',
      '',
      jasmine.objectContaining({ panelClass: ['snackbar-success'] })
    );
  });

  it('should open success snackbar with custom message', () => {
    service.openSuccessSnackBar('Custom success');
    expect(matSnackBarSpy.open).toHaveBeenCalledWith(
      'Custom success',
      '',
      jasmine.objectContaining({ panelClass: ['snackbar-success'] })
    );
  });

  it('should open error snackbar with default message', () => {
    service.openErrorSnackBar();
    expect(matSnackBarSpy.open).toHaveBeenCalledWith(
      'Error',
      '',
      jasmine.objectContaining({ panelClass: ['snackbar-error'] })
    );
  });

  it('should open error snackbar with custom message', () => {
    service.openErrorSnackBar('Custom error');
    expect(matSnackBarSpy.open).toHaveBeenCalledWith(
      'Custom error',
      '',
      jasmine.objectContaining({ panelClass: ['snackbar-error'] })
    );
  });
});
