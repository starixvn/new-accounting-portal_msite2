import { Component, Inject, OnInit, ViewEncapsulation } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog'

@Component({
    selector: 'app-confirm-dialog',
    templateUrl: './warning-dialog.component.html',
    styleUrls: ['./warning-dialog.component.scss'],
    encapsulation: ViewEncapsulation.None,
})
export class WarningDialogComponent implements OnInit {
    public message?: string;

    constructor(
        public dialogRef: MatDialogRef<WarningDialogComponent>,
        @Inject(MAT_DIALOG_DATA) public data: any
    ) {
        this.message = data.message;
    }

    ngOnInit(): void {
    }

    onOk() {
        this.dialogRef.close(true);
    }
}
