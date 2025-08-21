import { Component, OnInit } from '@angular/core';
import { ConfirmDialogService } from '../shared/components/dialog/dialog.service';
import { UpdateFlagRebateRetentionService } from './update-flag-rebate-retention.service';
import * as _ from 'lodash';

@Component({
  selector: 'app-control-sync-service',
  templateUrl: './update-flag-rebate-retention.component.html',
  styleUrls: ['./update-flag-rebate-retention.component.scss'],
})
export class UpdateFlagRebateRetentionComponent implements OnInit {
  public mode = 1;

  constructor(
    private readonly dialogService: ConfirmDialogService,
    private readonly updateFlagRebateRetentionService: UpdateFlagRebateRetentionService
  ) { }

  ngOnInit(): void {
    this.updateFlagRebateRetentionService.getInfo().subscribe((res) => {
      this.mode = res;
    });
  }

  openConfirmDialog(e: { checked: boolean, source: { checked: boolean } }) {
    const message = this.createDialogMessage(e.checked);
    const confirmText = e.checked ? 'ON' : 'OFF';
    const confirmStyle = e.checked ? 'btn-confirm-ok' : 'btn-confirm-delete';
    this.dialogService
      .openConfirm({ message, confirmText, confirmStyle })
      .afterClosed()
      .subscribe((confirm: boolean) => {
        if (confirm) {
          const value = e.checked ? 1 : 0;
          this.handleConfirm(value);
        } else {
          e.source.checked = this.mode === 1 ? true : false;
        }
      });
  }

  createDialogMessage(status: boolean) {
    const contentStatus = status ? 'ON' : 'OFF';
    const className = status ? 'blue' : 'red';
    const statusMessage = `<span class="${className}"><b>${contentStatus}</b></span>`;
    const message = `Are you sure you want to ${statusMessage} UUS currency on Rebate Retention Service?`;
    return message;
  }

  handleConfirm(mode: number) {
    this.updateFlagRebateRetentionService.updateFlag(mode).subscribe((res) => {
      this.mode = res;
    });
  }
}
