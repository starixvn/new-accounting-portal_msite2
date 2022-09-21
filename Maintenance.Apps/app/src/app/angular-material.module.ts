import { NgModule } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatDialogModule } from '@angular/material/dialog';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { MatIconModule } from '@angular/material/icon';

@NgModule({
  imports: [MatButtonModule, MatDialogModule, MatSnackBarModule, MatIconModule],
  exports: [MatButtonModule, MatDialogModule, MatSnackBarModule, MatIconModule],
})
export class AngularMaterialModule {}
