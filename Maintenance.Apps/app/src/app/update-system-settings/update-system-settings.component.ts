import { Component, OnInit } from '@angular/core';
import { ConfirmDialogService } from '../shared/components/confirm-dialog/confirm-dialog.service';
import { SnackbarService } from '../shared/services/snackbar.service';
import { SystemSettings } from './update-system-settings.model';
import { UpdateSystemSettingService } from './update-system-settings.service';

@Component({
  selector: 'app-update-system-settings',
  templateUrl: './update-system-settings.component.html',
  styleUrls: ['./update-system-settings.component.scss'],
})
export class UpdateSystemSettingsComponent implements OnInit {
  public systemSettings: SystemSettings[] | null = null;

  constructor(
    private service: UpdateSystemSettingService,
    private dialogService: ConfirmDialogService,
    private snackbarService: SnackbarService
  ) {}

  ngOnInit(): void {
    this.loadData();
  }

  loadData() {
    this.service.getAll().subscribe((data) => {
      this.systemSettings = data;
    });
  }

  openUpdateConfirmDialog(item: SystemSettings) {
    const message = `
        <div>DB Server: ${item.dbServer}<div>
        <div>VID: ${item.vId}<div>
        <div> Vname: ${item.vName}<div>
        <div>Vvalue: ${item.vValue}<div>
        <div>Are you sure you want to <b>UPDATE</b> this row?<div>
    `;

    this.dialogService
      .open({ message })
      .afterClosed()
      .subscribe((dialogResult) => {
        dialogResult && this.handleUpdate(item.dbServer);
      });
  }
  handleUpdate(dbServer: string) {
    this.service.update(dbServer).subscribe((res) => {
      this.snackbarService.openSuccessSnackBar();
      this.loadData();
    });
  }
}
