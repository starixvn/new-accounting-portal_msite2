import { Component, OnInit } from '@angular/core';
import { ConfirmDialogService } from '../shared/components/dialog/dialog.service';
import { isNumeric } from '../shared/helper';
import { SnackbarService } from '../shared/services/snackbar.service';
import { SyncTransIdService } from './sync-transid-directly.service';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatFormFieldModule } from '@angular/material/form-field';

@Component({
  selector: 'app-sync-transid-directly',
  templateUrl: './sync-transid-directly.component.html',
  styleUrls: ['./sync-transid-directly.component.scss'],
})
export class SyncTransidDirectlyComponent implements OnInit {
  siteDBOptions: string[] = [];
  siteDBSelected = '';
  tableOptions: string[] = [];
  tableSelected = '';
  subSiteDBOptions: any[] = [];
  subSiteDBsSelected: string[] = [];

  transIds: string = '';

  maxTransId = 2000;

  constructor(
    private dialogService: ConfirmDialogService,
    private syncTransIdService: SyncTransIdService,
    private snackbarService: SnackbarService
  ) {}

  ngOnInit(): void {
    this.syncTransIdService.getDbInfo().subscribe((res) => {
      this.siteDBOptions = res.dbSites;
      this.tableOptions = res.tables;
      this.subSiteDBOptions = res.subDbSites.map((x) => ({
        value: x,
        label: x,
        disabled: false,
      }));
    });
  }

  openConfirmDialog() {
    if (!this.siteDBSelected) {
      this.dialogService.openWarning({ message: 'Please select SiteDB field' });
      return;
    }

    if (!this.tableSelected) {
      this.dialogService.openWarning({
        message: 'Please select Table Update field',
      });
      return;
    }

    if (!this.validTransIds()) {
      this.dialogService.openWarning({
        message:
          "Invalid input. Your input need to be number and separated by comma (',') and max 2000",
      });
      return;
    }

    const message = `
            <div>DB Server: ${this.siteDBSelected}</div>
            <div>Action: Update RepUpdateTime</div>
            <div>Are you sure you want to <span class="blue">UPDATE</span> RepUpdateTime ?</div>
        `;

    this.dialogService
      .openConfirm({
        message,
        confirmText: 'Update',
        confirmStyle: 'btn-confirm-ok',
      })
      .afterClosed()
      .subscribe((confirm: Boolean) => {
        confirm && this.handleConfirm();
      });
  }

  validTransIds() {
    const ids = this.transIds.split(',');
    return (
      this.transIds && !ids.some((v) => !parseInt(v)) && ids.length <= 2000
    );
  }

  handleConfirm() {
    this.syncTransIdService
      .updateTransId({
        ServerName: this.siteDBSelected,
        TableName: this.tableSelected,
        TransIds: this.transIds,
        ToServerNames: this.subSiteDBsSelected,
      })
      .subscribe((res) => {
        if (!!res.errCode) {
          this.snackbarService.openErrorSnackBar(res.errMsg);
        } else {
          this.snackbarService.openSuccessSnackBar();
          this.transIds = '';
          this.tableSelected = '';
          this.siteDBSelected = '';
        }
      });
  }

  onSelectionChange(event: any): void {
    const value = event.value;
    if (value.includes('DBTXN')) {
      const dbTranOption = this.subSiteDBOptions.find(
        (x) => x.value === 'DBTXN'
      );

      this.subSiteDBOptions = [
        dbTranOption,
        ...this.subSiteDBOptions
          .filter((x) => x.value !== 'DBTXN')
          .map((x) => ({ ...x, disabled: true })),
      ];

      this.subSiteDBsSelected = ['DBTXN'];
    } else {
      this.subSiteDBOptions = this.subSiteDBOptions.map((x) => ({
        ...x,
        disabled: false,
      }));
    }
  }
}
