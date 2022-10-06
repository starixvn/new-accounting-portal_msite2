import { Component, Inject, OnInit, ViewEncapsulation } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ConfirmDialogService } from '../shared/components/confirm-dialog/confirm-dialog.service';
import { DeleteTempVariableNumberService } from './delete-temp-variable-number.service';
import { Match } from './match.model';

@Component({
  templateUrl: 'delete-temp-number-detail.component.html',
  styleUrls: ['delete-temp-number-detail.component.scss'],
  encapsulation: ViewEncapsulation.None,
})
export class DeleteTempVariableNumberDetailComponent implements OnInit {
  constructor(
    public dialogService: ConfirmDialogService,
    @Inject(MAT_DIALOG_DATA) public data: Match,
    private service: DeleteTempVariableNumberService
  ) {}

  get dataEntries() {
    return Object.entries(this.data);
  }

  ngOnInit(): void {}

  openDeleteConfirmDialog() {
    const message = `
        <div>DB Server: ${this.data.serverName}<div>
        <div>MatchId: ${this.data.matchId}<div>
        <div> EventName: ${this.data.eventName}<div>
        <div>Product: ${this.data.productName}<div>
        <div>Are you sure you want to <b>DELETE</b> this row?<div>
    `;

    const dialogRef = this.dialogService.open({
      message,
    });

    dialogRef.afterClosed().subscribe((dialogResult) => {
      dialogResult && this.handleDelete();
    });
  }

  handleDelete() {
    this.service.deleteItem(this.data);
  }
}
