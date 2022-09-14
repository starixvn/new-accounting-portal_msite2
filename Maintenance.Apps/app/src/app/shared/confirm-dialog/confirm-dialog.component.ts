import { Component, Inject, OnInit, ViewEncapsulation } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-confirm-dialog',
  templateUrl: './confirm-dialog.component.html',
  styleUrls: ['./confirm-dialog.component.scss'],
  encapsulation: ViewEncapsulation.None,
})
export class ConfirmDialogComponent implements OnInit {
  public title!: string;
  public message!: string;
  public confirmText!: string;
  public cancelText!: string;

  constructor(
    public dialogRef: MatDialogRef<ConfirmDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: ConfirmDialogData
  ) {
    console.log(data);
    this.title = data.title;
    this.message = data.message;
    this.cancelText = data.cancelText ?? 'Yes';
    this.confirmText = data.confirmText ?? 'No';
  }

  ngOnInit(): void {}

  onOk() {
    this.dialogRef.close(true);
  }

  onCancel() {
    this.dialogRef.close(false);
  }
}

export interface ConfirmDialogData {
  title: string;
  message: string;
  confirmText?: string;
  cancelText?: string;
}
