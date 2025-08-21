import { Component, Inject, ViewEncapsulation } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog'

@Component({
    selector: 'app-confirm-dialog',
    templateUrl: './warning-dialog.component.html',
    styleUrls: ['./warning-dialog.component.scss'],
    encapsulation: ViewEncapsulation.None,
})
export class WarningDialogComponent {
    public message?: string;

    constructor(
        public dialogRef: MatDialogRef<WarningDialogComponent>,
        @Inject(MAT_DIALOG_DATA) public data: { message: string }
    ) {
        this.message = data.message;
    }
    onOk() {
        this.dialogRef.close(true);
    }
}
