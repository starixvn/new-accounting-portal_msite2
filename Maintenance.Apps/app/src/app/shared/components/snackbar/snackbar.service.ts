import { MatSnackBar } from '@angular/material/snack-bar';
import { SnackbarComponent } from './snackbar.component';
import { Injectable } from '@angular/core';

@Injectable()
export class SnackbarService {
  constructor(private snackbar: MatSnackBar) {}

  openSnackBar() {
    this.snackbar.openFromComponent(SnackbarComponent, {
      horizontalPosition: 'right',
      verticalPosition: 'top',
      duration: 2000,
      panelClass: ['snack-style'],
    });
  }
}
