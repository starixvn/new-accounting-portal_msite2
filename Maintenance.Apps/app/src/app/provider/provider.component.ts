import { CommonModule, NgIf } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ProviderService } from './provider.service';
import { ProviderInfo } from './provider.model';
import { MatDialog } from '@angular/material/dialog';
import { MapProviderDialogComponent } from './map-provider-dialog/map-provider-dialog.component';
import { SnackbarService } from '../shared/services/snackbar.service';
import { ConfirmDialogService } from '../shared/components/dialog/dialog.service';

@Component({
  selector: 'app-provider',
  templateUrl: './provider.component.html',
  styleUrls: ['./provider.component.scss'],
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, NgIf]
})
export class ProviderComponent implements OnInit {

  private readonly providerService = inject(ProviderService);
  private readonly dialog = inject(MatDialog);
  private readonly snackbarService = inject(SnackbarService);
  private readonly confirmDialogService = inject(ConfirmDialogService);

  filterForm = new FormGroup({
    sportId: new FormControl<number | null>(null, [Validators.required]),
  });

  sportOptions: { id: number, name: string }[] = [];
  providers: ProviderInfo[] = [];
  allProviders: ProviderInfo[] = [];


  ngOnInit(): void {
    this.providerService.getAllSport().subscribe((res) => {
      this.sportOptions = res.map((sport) => ({
        id: sport.sportType,
        name: sport.sportName,
      }));
    });

    this.providerService.getAllProvider().subscribe((res) => {
      this.allProviders = res;
    });
  }

  submit() {
    if (this.filterForm.valid) {
      const { sportId } = this.filterForm.value;
      if (!sportId) {
        return;
      }

      this.providerService.getProvidersBySport(+sportId).subscribe((res) => {
        this.providers = res;
      });
    }
  }

  mapProvider() {
    const { sportId } = this.filterForm.value;
    if (!sportId) {
      return;
    }

    const sport = this.sportOptions.find((x) => x.id === +sportId);
    const otherProviders = this.allProviders.filter((x) => !this.providers.some((y) => y.sourceId === x.sourceId));

    const dialogRef = this.dialog.open(MapProviderDialogComponent, {
      data: {
        sport: sport,
        providers: otherProviders,
      },
      disableClose: true,
      panelClass: 'map-provider-dialog',
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        this.providerService.mapProviderToSport(+result.provider, sportId).subscribe((res) => {
          if (res.isSuccess) {
            this.snackbarService.openSuccessSnackBar('Provider mapped successfully');
            this.submit();
          } else {
            this.snackbarService.openErrorSnackBar('Failed to map provider');
          }
        });
      }
    });
  }

  unmapProvider(provider: ProviderInfo) {
    this.confirmDialogService.openConfirm({
      message: `Are you sure you want to unmap this provider? <b>${provider.sourceName}</b>`,
      title: 'UnMapping Provider',
      confirmText: 'Ok',
      cancelText: 'Cancel',
      confirmStyle: 'btn-confirm-ok',
    }).afterClosed().subscribe((result) => {
      if (result) {
        this.providerService.unmapProviderFromSport(provider.sourceId, provider.sportType).subscribe((res) => {
          this.snackbarService.openSuccessSnackBar('Provider unmapped successfully');
          this.submit();
        });
      }
    });
  }

}
