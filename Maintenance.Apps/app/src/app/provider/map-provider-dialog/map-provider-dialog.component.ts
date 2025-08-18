import { CommonModule } from '@angular/common';
import { Component, Inject, OnInit } from '@angular/core';
import { FormControl, ReactiveFormsModule, Validators } from '@angular/forms';
import { FormGroup } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-map-provider-dialog',
  templateUrl: './map-provider-dialog.component.html',
  styleUrls: ['./map-provider-dialog.component.scss'],
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule]
})
export class MapProviderDialogComponent implements OnInit {

  mapProviderForm = new FormGroup({
    provider: new FormControl<number | null>(null, [Validators.required]),
  });

  constructor(
    public dialogRef: MatDialogRef<MapProviderDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) { }

  ngOnInit(): void {
  }

  onCancel() {
    this.dialogRef.close();
  }

  onSubmit() {
    this.dialogRef.close(this.mapProviderForm.value);
  }

}
