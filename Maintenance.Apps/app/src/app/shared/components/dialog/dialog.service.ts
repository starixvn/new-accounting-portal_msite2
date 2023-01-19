import { Injectable } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ConfirmDialogComponent } from './confirm/confirm-dialog.component';
import { ConfirmDialogData } from './confirm/confirm-dialog.model';
import { WarningDialogComponent } from './warning/warning-dialog.component';


@Injectable()
export class ConfirmDialogService {
    constructor(private dialog: MatDialog) { }

    public openConfirm({
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

    public openWarning({ message }: any) {
        return this.dialog.open(WarningDialogComponent, {
            data: { message },
            width: '400px',
            disableClose: true,
            panelClass: 'warning-dialog',
        })
    }
}
