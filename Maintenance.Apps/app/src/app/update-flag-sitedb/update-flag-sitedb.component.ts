import { Component, OnInit } from '@angular/core';
import { UpdateFlagSitedbService } from './update-flag-sitedb.service';
import { SiteDB } from './site-db.model';
import { ConfirmDialogService } from '../shared/components/dialog/dialog.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
@Component({
  selector: 'app-update-flag-sitedb',
  templateUrl: './update-flag-sitedb.component.html',
  styleUrls: ['./update-flag-sitedb.component.scss'],
})
export class UpdateFlagSitedbComponent implements OnInit {
  public sites: SiteDB[] = [];
  public siteDBForm!: FormGroup;

  constructor(
    private dialogService: ConfirmDialogService,
    private updateFlagSitedbService: UpdateFlagSitedbService,
    private formBuilder: FormBuilder,
    private snackBar: MatSnackBar
  ) {
    this.initForm();
  }

  ngOnInit(): void {
    this.loadSiteDBs();
  }
  private initForm(): void {
    this.siteDBForm = this.formBuilder.group({
      siteDbGroupId: ['', Validators.required],
      siteName: ['', Validators.required],
      status: [true],
    });
  }

  onSubmit(): void {
    if (this.siteDBForm.valid) {
      const newSiteDB = this.siteDBForm.value;

      this.updateFlagSitedbService.addSiteDB(newSiteDB).subscribe({
        next: (response) => {
          this.sites.push(response);
          this.siteDBForm.reset({ status: true });
          this.snackBar.open('SiteDB added successfully', 'Close', {
            duration: 3000,
          });
        },
        error: (error) => {
          this.snackBar.open('Error adding SiteDB: ' + error.message, 'Close', {
            duration: 5000,
          });
        },
      });
    }
  }

  loadSiteDBs(): void {
    this.updateFlagSitedbService.getSiteDBs().subscribe((sites: SiteDB[]) => {
      this.sites = sites;
    });
  }

  openConfirmDialog(e: any, site: SiteDB): void {
    const message = this.createDialogMessage(e.checked, site.siteName);
    const confirmText = e.checked ? 'ON' : 'OFF';
    const confirmStyle = e.checked ? 'btn-confirm-ok' : 'btn-confirm-delete';

    this.dialogService
      .openConfirm({ message, confirmText, confirmStyle })
      .afterClosed()
      .subscribe((confirm: Boolean) => {
        if (confirm) {
          site.status = e.checked;
          this.updateSiteDBStatus(site);
        } else {
          e.source.checked = !e.checked;
        }
      });
  }

  createDialogMessage(status: boolean, siteName: string): string {
    const contentStatus = status ? 'ON' : 'OFF';
    const className = status ? 'blue' : 'red';
    const statusMessage = `<span class="${className}"><b>${contentStatus}</b></span>`;
    const message = `Are you sure you want to turn ${statusMessage} ${siteName}?`;
    return message;
  }

  updateSiteDBStatus(site: SiteDB): void {
    this.updateFlagSitedbService
      .updateSiteDBStatus(site)
      .subscribe((updatedSite: SiteDB) => {
        const index = this.sites.findIndex(
          (site) => site.siteDbGroupId === updatedSite.siteDbGroupId
        );
        if (index !== -1) {
          this.sites[index] = updatedSite;
        }
      });
  }
}
