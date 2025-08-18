import { Component, OnInit } from '@angular/core';
import { ConfirmDialogService } from '../shared/components/dialog/dialog.service';
import { SnackbarService } from '../shared/services/snackbar.service';
import { SyncTransIdService } from './sync-transid-directly.service';
import { SynMatch } from './sync-transid-directly.model';

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

  subSiteDBOptions: { value: string, label: string, disabled: boolean }[] = [];
  subSiteDBsSelected: string[] = [];

  typeUpdate = 'transId';

  transIds = '';

  matchId = '';

  maxTransId = 2000;

  syncMatches: SynMatch[] | null = null;

  constructor(
    private readonly dialogService: ConfirmDialogService,
    private readonly syncTransIdService: SyncTransIdService,
    private readonly snackbarService: SnackbarService
  ) { }

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

  onSelectionChangeTypeUpdate() {
    this.resetState();
  }

  resetState() {
    this.matchId = '';
    this.subSiteDBsSelected = [];
    this.transIds = '';
    this.syncMatches = null;
    this.subSiteDBOptions = this.subSiteDBOptions.map((x) => ({
      ...x,
      disabled: false,
    }));
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

    const messageSyncTransIdCentralDb = `
            <div>DB Server: ${this.siteDBSelected}</div>
            <div>Action: Update RepUpdateTime</div>
            <div>Are you sure you want to <span class="blue">UPDATE</span> RepUpdateTime ?</div>
        `;

    const messageSyncBatch = `
        <div>DB Server: ${this.siteDBSelected}</div>
        <div>Action: Usync data from ${this.siteDBSelected
      } to ${this.subSiteDBsSelected.join(', ')}</div>
        <div>Are you sure you want to sync data?</div>
    `;

    const message =
      this.typeUpdate === 'transId' &&
        (!this.subSiteDBsSelected.length ||
          this.subSiteDBsSelected.filter((x) => x === 'DBTXN').length === 1)
        ? messageSyncTransIdCentralDb
        : messageSyncBatch;

    this.dialogService
      .openConfirm({
        message,
        confirmText: 'Update',
        confirmStyle: 'btn-confirm-ok',
      })
      .afterClosed()
      .subscribe((confirm: boolean) => {
        confirm && this.handleConfirm();
      });
  }

  validTransIds() {
    const ids = this.transIds.split(',');
    return this.transIds && !ids.some((v) => !BigInt(v)) && ids.length <= this.maxTransId;
  }

  handleConfirm() {
    this.syncTransIdService
      .updateTransId({
        serverName: this.siteDBSelected,
        tableName: this.tableSelected,
        transIds: this.transIds.replace(/\s/g, ''),
        toServerNames: this.subSiteDBsSelected,
      })
      .subscribe((res) => {
        if (res.errCode) {
          this.snackbarService.openErrorSnackBar(res.errMsg);
        } else {
          this.snackbarService.openSuccessSnackBar();
          this.resetState();
        }
      });
  }

  onSelectionChange(event: { value: string }): void {
    const value = event.value;
    if (value.includes('DBTXN')) {
      const dbTranOption = this.subSiteDBOptions.find(
        (x) => x.value === 'DBTXN'
      );

      this.subSiteDBOptions = [
        dbTranOption as { value: string, label: string, disabled: boolean },
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

  getSynTicketForMatch() {
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
    this.syncTransIdService
      .getSynTicketForMatch({
        matchId: +this.matchId,
        tableName: this.tableSelected,
        typeUpdate: this.typeUpdate,
        serverName: this.siteDBSelected,
      })
      .subscribe((res) => {
        this.syncMatches = res;
      });
  }

  updateMatchBatchTransId(item: SynMatch) {
    if (!this.subSiteDBsSelected.length) {
      this.dialogService.openWarning({
        message: 'Please select SubDB, you want to sync!',
      });
      return;
    }


    this.syncTransIdService
      .updateBatchTransId({
        ...item,
        fromTransId: item.minTransId,
        toTransId: item.maxTransId,
        toServerNames: this.subSiteDBsSelected,
      })
      .subscribe((res) => {
        if (res.isSuccess) {
          item.isUpdated = res.isSuccess;
          this.snackbarService.openSuccessSnackBar();
        } else {
          this.snackbarService.openErrorSnackBar(res.errMsg);
        }
      });
  }
}
