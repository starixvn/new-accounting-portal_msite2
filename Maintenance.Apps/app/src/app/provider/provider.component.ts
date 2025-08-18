import { CommonModule, NgIf } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ProviderService } from './provider.service';
import { ProviderInfo } from './provider.model';
import { MatDialog } from '@angular/material/dialog';
import { MapProviderDialogComponent } from './map-provider-dialog/map-provider-dialog.component';

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

  filterForm = new FormGroup({
    sportId: new FormControl<number | null>(null, [Validators.required]),
  });

  sportOptions: { id: number, name: string }[] = [];
  providers: ProviderInfo[] = [];


  ngOnInit(): void {
    this.providerService.getAllSport().subscribe((res) => {
      this.sportOptions = res.map((sport) => ({
        id: sport.sportType,
        name: sport.sportName,
      }));
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

    const dialogRef = this.dialog.open(MapProviderDialogComponent, {
      data: {
        sport: sport,
        providers: this.providers,
      },
      disableClose: true,
    });

    dialogRef.afterClosed().subscribe((result) => {
      // console.log(result);
      if (result) {
        // this.providerService.mapProvider(result.).subscribe((res) => {
        //   console.log(res);
        // });
      }
    });
  }

}
