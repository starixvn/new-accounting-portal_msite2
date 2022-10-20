import { Injectable } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ConfirmDialogComponent } from './confirm-dialog.component';
import { ConfirmDialogData } from './confirm-dialog.model';

@Injectable()
export class ConfirmDialogService {
  constructor(private dialog: MatDialog) {}

  public open({
    message,
    title,
    confirmText,
    cancelText,
    confirmStyle,
  }: ConfirmDialogData) {
    return this.dialog.open(ConfirmDialogComponent, {
      data: { message, title, confirmText, cancelText, confirmStyle },
      width: '400px',
      disableClose: true,
      panelClass: 'confirm-dialog',
    });
  }
}
