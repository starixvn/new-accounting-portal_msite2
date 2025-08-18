import { MatSnackBar, MatSnackBarConfig } from '@angular/material/snack-bar';
import { Injectable } from '@angular/core';

@Injectable()
export class SnackbarService {
  private config: MatSnackBarConfig = {
    horizontalPosition: 'right',
    verticalPosition: 'top',
    duration: 2000,
  };

  constructor(private readonly snackbar: MatSnackBar) { }

  openSuccessSnackBar(message?: string) {
    this.snackbar.open(message ?? 'Successful', '', {
      panelClass: ['snackbar-success'],
      ...this.config,
    });
  }

  openErrorSnackBar(message?: string) {
    this.snackbar.open(message ?? 'Error', '', {
      panelClass: ['snackbar-error'],
      ...this.config,
    });
  }
}
